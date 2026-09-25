using RestaurantTraining.Web.Models;
namespace RestaurantTraining.Web.Services.CertificateManagement;

public interface ICertificateService
{
    Task<CertificateSettingModel?> GetSettingAsync(int moduleId);

    Task<CertificateSaveResult> SaveSettingAsync(SaveCertificateSettingRequest request);
}


