using RestaurantTraining.Web.Models;

namespace RestaurantTraining.Web.Services.SettingsManagement;

public interface ISettingsService
{
    Task<ChangePasswordResult> ChangePasswordAsync(ChangePasswordRequest request);

    Task<UpdateEmailResult> UpdateEmailAsync(UpdateEmailRequest request);

    Task<DeleteAccountResult> DeleteAccountAsync(DeleteAccountRequest request);
    // ISettingsService.cs (add these two)
  
}