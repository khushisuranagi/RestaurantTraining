namespace RestaurantTraining.Application.Features.LearnerModules.Queries.GetAssignedModules
{
    // Matches the Blazor LearnerModuleModel (field names unchanged).
    public class AssignedModuleDto
    {
        public int ModuleId { get; set; }
        public string ModuleName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
