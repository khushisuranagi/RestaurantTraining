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
                    CreatedAt = u.CreatedAt
                }
            ).FirstOrDefaultAsync(cancellationToken);
        }
    }
}
