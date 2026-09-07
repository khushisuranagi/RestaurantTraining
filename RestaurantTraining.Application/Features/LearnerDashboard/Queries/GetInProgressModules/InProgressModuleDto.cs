namespace RestaurantTraining.Application.Features.LearnerDashboard.Queries.GetInProgressModules
{
    public class InProgressModuleDto
    {
        public int ModuleId { get; set; }
        public string ModuleName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int CompletedLessons { get; set; }
        public int TotalLessons { get; set; }
        public DateTime? LastAccessedAt { get; set; }
    }
}
