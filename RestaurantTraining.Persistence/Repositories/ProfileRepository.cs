using Microsoft.EntityFrameworkCore;
using RestaurantTraining.Application.Common.Interfaces;

namespace RestaurantTraining.Persistence.Repositories
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly AppDbContext _context;

        public ProfileRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ProfileInfo?> GetProfileAsync(
            int userId, CancellationToken cancellationToken)
        {
            return await (
                from u in _context.Users
                join r in _context.Roles on u.RoleId equals r.RoleId
                where u.UserId == userId
                select new ProfileInfo
                {
                    UserId = u.UserId,
                    FullName = u.FullName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    Role = r.RoleName,
                    IsActive = u.IsActive,
                    CreatedAt = u.CreatedAt,
                    LastLoginAt = u.LastLoginAt
                }
            ).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<bool> UpdateProfileAsync(
            int userId, string fullName, string phoneNumber, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == userId, cancellationToken);

            if (user is null)
                return false;

            user.FullName = fullName;
            user.PhoneNumber = phoneNumber;

            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
