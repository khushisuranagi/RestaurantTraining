using RestaurantTraining.Application.Features.RoleModules.Queries.GetModuleAssignments;
using RestaurantTraining.Domain.Entities;

namespace RestaurantTraining.Application.Common.Interfaces
{
    public interface IRoleModuleRepository
    {
        Task<bool> ExistsAsync(int roleId, int moduleId, CancellationToken cancellationToken);

        Task AssignAsync(RoleModule roleModule, CancellationToken cancellationToken);

        Task<bool> RemoveAsync(int roleId, int moduleId, CancellationToken cancellationToken);

        // Returns the roles a module is assigned to (with role names).
        Task<List<ModuleAssignmentDto>> GetAssignmentsByModuleAsync(
            int moduleId, CancellationToken cancellationToken);
    }
}