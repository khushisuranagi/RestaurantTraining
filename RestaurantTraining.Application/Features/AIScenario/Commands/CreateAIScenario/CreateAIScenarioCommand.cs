using MediatR;

namespace RestaurantTraining.Application.Features.AIScenarios.Commands.CreateAIScenario
{
    public class CreateAIScenarioCommand : IRequest<CreateAIScenarioResponse>
    {
        public int ModuleId { get; set; }

        public int RoleId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string ScenarioPrompt { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public decimal PassingScore { get; set; }
    }
}