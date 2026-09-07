namespace RestaurantTraining.Application.Features.LearnerDashboard.Queries.GetMyModulesOverview
{
    // Matches the Blazor LearnerModuleOverviewModel (field names unchanged).
    public class ModuleOverviewDto
    {
        public int ModuleId { get; set; }
        public string ModuleName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool HasStarted { get; set; }
        public bool IsCompleted { get; set; }
        public int CompletedLessons { get; set; }
        public int TotalLessons { get; set; }
        public int ProgressPercent { get; set; }
    }
}
