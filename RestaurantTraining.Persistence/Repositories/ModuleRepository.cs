using Microsoft.EntityFrameworkCore;
using RestaurantTraining.Application.Common.Interfaces;
using RestaurantTraining.Domain.Entities;

namespace RestaurantTraining.Persistence.Repositories
{
    public class ModuleRepository : IModuleRepository
    {
        private readonly AppDbContext _context;

        public ModuleRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddModuleAsync(
            Module module,
            CancellationToken cancellationToken)
        {
            await _context.Modules.AddAsync(module, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            // After saving, EF fills in the new ModuleId for us.
            return module.ModuleId;
        }

        public async Task<List<Module>> GetAllModulesAsync(
            CancellationToken cancellationToken)
        {
            return await _context.Modules
                .ToListAsync(cancellationToken);
        }

        public async Task<Module?> GetModuleByIdAsync(
            int moduleId,
            CancellationToken cancellationToken)
        {
            return await _context.Modules
                .FirstOrDefaultAsync(
                    x => x.ModuleId == moduleId,
                    cancellationToken);
        }

        public async Task UpdateModuleAsync(
            Module module,
            CancellationToken cancellationToken)
        {
            _context.Modules.Update(module);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteModuleAsync(
            Module module,
            CancellationToken cancellationToken)
        {
            _context.Modules.Remove(module);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
