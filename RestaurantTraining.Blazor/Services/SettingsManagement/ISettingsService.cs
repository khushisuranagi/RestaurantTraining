using RestaurantTraining.Blazor.Models;

namespace RestaurantTraining.Blazor.Services.SettingsManagement;

public interface ISettingsService
{
    Task<ChangePasswordResult> ChangePasswordAsync(ChangePasswordRequest request);

    Task<UpdateEmailResult> UpdateEmailAsync(UpdateEmailRequest request);

    Task<DeleteAccountResult> DeleteAccountAsync(DeleteAccountRequest request);
}