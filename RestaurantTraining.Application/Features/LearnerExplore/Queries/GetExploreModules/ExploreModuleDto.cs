namespace RestaurantTraining.Application.Features.LearnerExplore.Queries.GetExploreModules
{
    // One optional (unassigned) module shown in the Explore list.
    public class ExploreModuleDto
    {
        public int ModuleId { get; set; }
        public string ModuleName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? CoverImageData { get; set; }
        public string? CoverImageContentType { get; set; }
    }
}
