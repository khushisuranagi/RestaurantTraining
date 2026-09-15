namespace RestaurantTraining.Application.Common.Interfaces
{
    // Data access for the learner "Explore" section:
    // optional modules that are NOT assigned to the learner's role.
    public interface ILearnerExploreRepository
    {
        // Active modules that are not assigned to the given role.
        Task<List<ExploreModuleInfo>> GetUnassignedModulesAsync(
            string roleName, CancellationToken cancellationToken);

        // Lesson content for any active module (no role gate — read only).
        // Reuses the shared ModuleContentInfo shape. IsCompleted is always false
        // here because Explore doesn't track progress.
        Task<ModuleContentInfo?> GetModuleContentAsync(
            int moduleId, CancellationToken cancellationToken);
    }

    public class ExploreModuleInfo
    {
        public int ModuleId { get; set; }
        public string ModuleName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? CoverImageData { get; set; }
        public string? CoverImageContentType { get; set; }
    }
}
