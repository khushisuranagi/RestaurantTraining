using Microsoft.EntityFrameworkCore;
using RestaurantTraining.Application.Common.Interfaces;
using RestaurantTraining.Application.Features.RoleModules.Queries.GetModuleAssignments;
using RestaurantTraining.Domain.Entities;

namespace RestaurantTraining.Persistence.Repositories
{
    public class RoleModuleRepository : IRoleModuleRepository
    {
        private readonly AppDbContext _context;

        public RoleModuleRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ExistsAsync(
            int roleId, int moduleId, CancellationToken cancellationToken)
        {
            return await _context.RoleModules.AnyAsync(
                x => x.RoleId == roleId && x.ModuleId == moduleId, cancellationToken);
        }

        public async Task AssignAsync(
            RoleModule roleModule, CancellationToken cancellationToken)
        {
            await _context.RoleModules.AddAsync(roleModule, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> RemoveAsync(
            int roleId, int moduleId, CancellationToken cancellationToken)
        {
            var existing = await _context.RoleModules.FirstOrDefaultAsync(
                x => x.RoleId == roleId && x.ModuleId == moduleId, cancellationToken);

            if (existing == null)
                return false;

            _context.RoleModules.Remove(existing);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<List<ModuleAssignmentDto>> GetAssignmentsByModuleAsync(
            int moduleId, CancellationToken cancellationToken)
        {
            return await (
                from rm in _context.RoleModules
                join r in _context.Roles on rm.RoleId equals r.RoleId
                where rm.ModuleId == moduleId
                orderby r.RoleName
                select new ModuleAssignmentDto
                {
                    RoleId = rm.RoleId,
                    RoleName = r.RoleName,
                    AssignedAt = rm.AssignedAt
                }).ToListAsync(cancellationToken);
        }
    }
}