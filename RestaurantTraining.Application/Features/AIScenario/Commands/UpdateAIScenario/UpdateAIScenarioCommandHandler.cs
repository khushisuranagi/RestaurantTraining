using MediatR;
using RestaurantTraining.Application.Common.Interfaces;

namespace RestaurantTraining.Application.Features.AIScenarios.Commands.UpdateAIScenario
{
    public class UpdateAIScenarioCommandHandler
        : IRequestHandler<UpdateAIScenarioCommand, UpdateAIScenarioResponse>
    {
        private readonly IAIScenarioRepository _aiScenarioRepository;

        public UpdateAIScenarioCommandHandler(
            IAIScenarioRepository aiScenarioRepository)
        {
            _aiScenarioRepository = aiScenarioRepository;
        }

        public async Task<UpdateAIScenarioResponse> Handle(
            UpdateAIScenarioCommand request,
            CancellationToken cancellationToken)
        {
            var scenario =
                await _aiScenarioRepository.GetAIScenarioByIdAsync(
                    request.ScenarioId,
                    cancellationToken);

            if (scenario == null)
            {
                return new UpdateAIScenarioResponse
                {
                    Success = false,
                    Message = "AI scenario not found."
                };
            }

            if (string.IsNullOrWhiteSpace(request.Title))
            {
                return new UpdateAIScenarioResponse
                {
                    Success = false,
                    Message = "Scenario title is required."
                };
            }

            scenario.ModuleId = request.ModuleId;
            scenario.RoleId = request.RoleId;
            scenario.Title = request.Title;
            scenario.Description = request.Description;
            scenario.ScenarioPrompt = request.ScenarioPrompt;
            scenario.Category = request.Category;
            scenario.PassingScore = request.PassingScore;
            scenario.IsActive = request.IsActive;

            await _aiScenarioRepository.UpdateAIScenarioAsync(
                scenario,
                cancellationToken);

            return new UpdateAIScenarioResponse
            {
                Success = true,
                Message = "AI scenario updated successfully.",
                ScenarioId = scenario.ScenarioId
            };
        }
    }
}