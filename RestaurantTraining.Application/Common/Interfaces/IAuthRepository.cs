using RestaurantTraining.Domain.Entities;

namespace RestaurantTraining.Application.Common.Interfaces
{
    public interface IAuthRepository
    {
        Task<User?> GetUserByEmailAsync(
            string email,
            CancellationToken cancellationToken);

        Task<bool> EmailExistsAsync(
            string email,
            CancellationToken cancellationToken);

        Task<Role?> GetRoleByIdAsync(
            int roleId,
            CancellationToken cancellationToken);

        Task<List<Role>> GetSelfRegistrationRolesAsync(
            CancellationToken cancellationToken);

        Task AddUserAsync(
            User user,
            CancellationToken cancellationToken);
    }
}
