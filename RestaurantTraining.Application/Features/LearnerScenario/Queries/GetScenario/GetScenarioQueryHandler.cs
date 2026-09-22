using MediatR;
using RestaurantTraining.Application.Common.Interfaces;
using RestaurantTraining.Domain.Entities;

namespace RestaurantTraining.Application.Features.LearnerScenario.Queries.GetScenario
{
    public class GetScenarioQueryHandler
        : IRequestHandler<GetScenarioQuery, ScenarioResult>
    {
        private readonly IAIScenarioRepository _repository;
        private readonly IAiRoleplay _roleplay;

        public GetScenarioQueryHandler(
            IAIScenarioRepository repository,
            IAiRoleplay roleplay)
        {
            _repository = repository;
            _roleplay = roleplay;
        }

        public async Task<ScenarioResult> Handle(
            GetScenarioQuery request, CancellationToken cancellationToken)
        {
            // Resolve the role + module (also validates they exist).
            var ctx = await _repository.GetGenerationContextAsync(
                request.ModuleId, request.RoleName, cancellationToken);

            if (ctx is null)
                return new ScenarioResult { NotAvailable = true };

            // Reuse the stored scenario for this (module, role) if one exists.
            var scenario = await _repository.GetActiveScenarioAsync(
                request.ModuleId, ctx.RoleId, cancellationToken);

            if (scenario is null)
            {
                // Otherwise generate one and store it for everyone of this role.
                var generated = await _roleplay.GenerateScenarioAsync(
                    ctx.ModuleTitle, ctx.RoleName, cancellationToken);

                if (generated is null)
                    return new ScenarioResult { NotAvailable = true };

                var toSave = new AIScenario
                {
                    ModuleId = request.ModuleId,
                    RoleId = ctx.RoleId,
                    Title = generated.Title,
                    Description = generated.Description,
                    ScenarioPrompt = generated.ScenarioPrompt,
                    Category = generated.Category,
                    PassingScore = 0,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                toSave.ScenarioId = await _repository.AddScenarioAsync(toSave, cancellationToken);
                scenario = toSave;
            }

            var alreadyPassed = await _repository.HasPassedScenarioAsync(
                request.UserId, scenario.ScenarioId, cancellationToken);

            return new ScenarioResult
            {
                Scenario = new ScenarioDto
                {
                    ScenarioId = scenario.ScenarioId,
                    Title = scenario.Title,
                    Description = scenario.Description,
                    AlreadyPassed = alreadyPassed
                }
            };
        }
    }
}
