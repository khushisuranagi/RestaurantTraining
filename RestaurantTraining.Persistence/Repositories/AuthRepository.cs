using Microsoft.EntityFrameworkCore;
using RestaurantTraining.Application.Common.Interfaces;
using RestaurantTraining.Domain.Entities;

namespace RestaurantTraining.Persistence.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly AppDbContext _context;

        public AuthRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByEmailAsync(
            string email,
            CancellationToken cancellationToken)
        {
            return await _context.Users
                .Include(x => x.Role)
                .FirstOrDefaultAsync(
                    x => x.Email == email,
                cancellationToken);
        }

        public Task<bool> EmailExistsAsync(
            string email,
            CancellationToken cancellationToken)
        {
            return _context.Users.AnyAsync(x => x.Email == email, cancellationToken);
        }

        public Task<Role?> GetRoleByIdAsync(
            int roleId,
            CancellationToken cancellationToken)
        {
            return _context.Roles.FirstOrDefaultAsync(
                x => x.RoleId == roleId && x.IsActive,
                cancellationToken);
        }

        public Task<List<Role>> GetSelfRegistrationRolesAsync(
            CancellationToken cancellationToken)
        {
            return _context.Roles
                .Where(x => x.IsActive &&  x.RoleName != "Content Creator")
                .OrderBy(x => x.RoleName)
                .ToListAsync(cancellationToken);
        }

        public async Task AddUserAsync(
            User user,
            CancellationToken cancellationToken)
        {
            await _context.Users.AddAsync(user, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
