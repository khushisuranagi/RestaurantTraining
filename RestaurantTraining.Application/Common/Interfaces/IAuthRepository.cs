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

        Task UpdateUserAsync(
             User user,
             CancellationToken cancellationToken);

        Task<User?> GetUserByIdAsync(
    int userId,
    CancellationToken cancellationToken);

        // Permanently deletes the user and every row that references them.
        Task DeleteAccountAsync(
            int userId,
            CancellationToken cancellationToken);
    }
}
