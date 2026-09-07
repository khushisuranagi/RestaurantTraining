using RestaurantTraining.Domain.Entities;

namespace RestaurantTraining.Application.Common.Interfaces
{
    public interface IModuleCertificateSettingRepository
    {
        Task<ModuleCertificateSetting?> GetByModuleIdAsync(
            int moduleId,
            CancellationToken cancellationToken);

        Task<int> AddAsync(
            ModuleCertificateSetting setting,
            CancellationToken cancellationToken);

        Task UpdateAsync(
            ModuleCertificateSetting setting,
            CancellationToken cancellationToken);
    }
}