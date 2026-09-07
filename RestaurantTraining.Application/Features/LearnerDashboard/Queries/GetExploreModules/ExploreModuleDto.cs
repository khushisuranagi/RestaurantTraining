namespace RestaurantTraining.Application.Features.LearnerDashboard.Queries.GetExploreModules
{
    public class ExploreModuleDto
    {
        public int ModuleId { get; set; }
        public string ModuleName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime AssignedAt { get; set; }
    }
}
