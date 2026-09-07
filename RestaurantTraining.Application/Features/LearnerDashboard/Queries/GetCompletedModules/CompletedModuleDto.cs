namespace RestaurantTraining.Application.Features.LearnerDashboard.Queries.GetCompletedModules
{
    public class CompletedModuleDto
    {
        public int ModuleId { get; set; }
        public string ModuleName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime? CompletedAt { get; set; }
    }
}
