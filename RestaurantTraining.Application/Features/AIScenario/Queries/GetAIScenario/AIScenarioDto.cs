namespace RestaurantTraining.Application.Features.AIScenarios.Queries.GetAIScenarios
{
    public class AIScenarioDto
    {
        public int ScenarioId { get; set; }

        public int ModuleId { get; set; }

        public int RoleId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string ScenarioPrompt { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public decimal PassingScore { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}