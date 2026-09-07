using RestaurantTraining.Blazor.Models;
namespace RestaurantTraining.Blazor.Services.CertificateManagement;

public interface ICertificateService
{
    Task<CertificateSettingModel?> GetSettingAsync(int moduleId);

    Task<CertificateSaveResult> SaveSettingAsync(SaveCertificateSettingRequest request);
}


