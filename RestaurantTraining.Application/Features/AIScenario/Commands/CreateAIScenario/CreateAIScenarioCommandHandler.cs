using MediatR;
using RestaurantTraining.Application.Common.Interfaces;
using RestaurantTraining.Domain.Entities;

namespace RestaurantTraining.Application.Features.AIScenarios.Commands.CreateAIScenario
{
    public class CreateAIScenarioCommandHandler
        : IRequestHandler<CreateAIScenarioCommand, CreateAIScenarioResponse>
    {
        private readonly IAIScenarioRepository _aiScenarioRepository;

        public CreateAIScenarioCommandHandler(
            IAIScenarioRepository aiScenarioRepository)
        {
            _aiScenarioRepository = aiScenarioRepository;
        }

        public async Task<CreateAIScenarioResponse> Handle(
            CreateAIScenarioCommand request,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Title))
            {
                return new CreateAIScenarioResponse
                {
                    Success = false,
                    Message = "Scenario title is required."
                };
            }

            var scenario = new AIScenario
            {
                ModuleId = request.ModuleId,
                RoleId = request.RoleId,
                Title = request.Title,
                Description = request.Description,
                ScenarioPrompt = request.ScenarioPrompt,
                Category = request.Category,
                PassingScore = request.PassingScore,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            var newScenarioId =
                await _aiScenarioRepository.AddAIScenarioAsync(
                    scenario,
                    cancellationToken);

            return new CreateAIScenarioResponse
            {
                Success = true,
                Message = "AI scenario created successfully.",
                ScenarioId = newScenarioId
            };
        }
    }
}