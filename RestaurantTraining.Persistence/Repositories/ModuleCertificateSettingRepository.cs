using Microsoft.EntityFrameworkCore;
using RestaurantTraining.Application.Common.Interfaces;
using RestaurantTraining.Domain.Entities;

namespace RestaurantTraining.Persistence.Repositories
{
    public class ModuleCertificateSettingRepository
        : IModuleCertificateSettingRepository
    {
        private readonly AppDbContext _context;

        public ModuleCertificateSettingRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ModuleCertificateSetting?> GetByModuleIdAsync(
            int moduleId,
            CancellationToken cancellationToken)
        {
            return await _context.ModuleCertificateSettings
                .FirstOrDefaultAsync(
                    x => x.ModuleId == moduleId,
                    cancellationToken);
        }

        public async Task<int> AddAsync(
            ModuleCertificateSetting setting,
            CancellationToken cancellationToken)
        {
            await _context.ModuleCertificateSettings.AddAsync(
                setting,
                cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return setting.SettingId;
        }

        public async Task UpdateAsync(
            ModuleCertificateSetting setting,
            CancellationToken cancellationToken)
        {
            _context.ModuleCertificateSettings.Update(setting);

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}