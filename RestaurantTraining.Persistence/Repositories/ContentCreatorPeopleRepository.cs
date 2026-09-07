using Microsoft.EntityFrameworkCore;
using RestaurantTraining.Application.Common.Interfaces;

namespace RestaurantTraining.Persistence.Repositories
{
    public class ContentCreatorPeopleRepository : IContentCreatorPeopleRepository
    {
        private readonly AppDbContext _context;

        public ContentCreatorPeopleRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<PeopleRoleInfo>> GetRolesAsync(
            CancellationToken cancellationToken)
        {
            return await _context.Roles
                .Select(r => new PeopleRoleInfo
                {
                    RoleId = r.RoleId,
                    RoleName = r.RoleName
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<List<PeopleUserInfo>> GetUsersAsync(
            CancellationToken cancellationToken)
        {
            return await _context.Users
                .Select(u => new PeopleUserInfo
                {
                    UserId = u.UserId,
                    FullName = u.FullName,
                    Email = u.Email,
                    RoleId = u.RoleId
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<List<PeopleRoleModuleInfo>> GetRoleModulesAsync(
            CancellationToken cancellationToken)
        {
            return await (
                from rm in _context.RoleModules
                join m in _context.Modules on rm.ModuleId equals m.ModuleId
                select new PeopleRoleModuleInfo
                {
                    RoleId = rm.RoleId,
                    ModuleId = m.ModuleId,
                    ModuleName = m.ModuleName
                })
                .ToListAsync(cancellationToken);
        }
    }
}
