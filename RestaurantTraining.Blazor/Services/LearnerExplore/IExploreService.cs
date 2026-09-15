using RestaurantTraining.Blazor.Models;

namespace RestaurantTraining.Blazor.Services.LearnerExplore;

public interface IExploreService
{
    // Optional modules not assigned to the learner's role.
    Task<List<ExploreModuleModel>> GetExploreModulesAsync();

    // Read-only lesson content for one explore module.
    Task<LearnerModuleContentModel?> GetExploreModuleAsync(int moduleId);
}
