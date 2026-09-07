using MediatR;
using RestaurantTraining.Application.Common.Interfaces;

namespace RestaurantTraining.Application.Features.AIScenarios.Queries.GetAIScenarios
{
    public class GetAIScenariosQueryHandler
        : IRequestHandler<GetAIScenariosQuery, List<AIScenarioDto>>
    {
        private readonly IAIScenarioRepository _aiScenarioRepository;

        public GetAIScenariosQueryHandler(
            IAIScenarioRepository aiScenarioRepository)
        {
            _aiScenarioRepository = aiScenarioRepository;
        }

        public async Task<List<AIScenarioDto>> Handle(
            GetAIScenariosQuery request,
            CancellationToken cancellationToken)
        {
            var scenarios =
                await _aiScenarioRepository.GetAllAIScenariosAsync(
                    cancellationToken);

            return scenarios.Select(x => new AIScenarioDto
            {
                ScenarioId = x.ScenarioId,
                ModuleId = x.ModuleId,
                RoleId = x.RoleId,
                Title = x.Title,
                Description = x.Description,
                ScenarioPrompt = x.ScenarioPrompt,
                Category = x.Category,
                PassingScore = x.PassingScore,
                IsActive = x.IsActive,
                CreatedAt = x.CreatedAt
            }).ToList();
        }
    }
}