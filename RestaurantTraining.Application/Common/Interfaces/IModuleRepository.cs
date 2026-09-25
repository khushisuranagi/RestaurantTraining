using RestaurantTraining.Domain.Entities;

namespace RestaurantTraining.Application.Common.Interfaces
{
    public interface IModuleRepository
    {
        Task<int> AddModuleAsync(
            Module module,
            CancellationToken cancellationToken);

        Task<List<Module>> GetAllModulesAsync(
            CancellationToken cancellationToken);

        Task<Module?> GetModuleByIdAsync(
            int moduleId,
            CancellationToken cancellationToken);

        Task UpdateModuleAsync(
            Module module,
            CancellationToken cancellationToken);

        Task DeleteModuleAsync(
            Module module,
            CancellationToken cancellationToken);

        Task<int> GetLessonCountForModuleAsync(
    int moduleId,
    CancellationToken cancellationToken);

        Task DeleteModuleWithChildrenAsync(
    int moduleId,
    CancellationToken cancellationToken);

    }
}
