using System.Net.Http.Json;
using RestaurantTraining.Blazor.Models;

namespace RestaurantTraining.Blazor.Services.SettingsManagement;

public class SettingsService : ISettingsService
{
    private readonly HttpClient _http;

    public SettingsService(HttpClient http)
    {
        _http = http;
    }

    public async Task<ChangePasswordResult> ChangePasswordAsync(ChangePasswordRequest request)
    {
        var response = await _http.PostAsJsonAsync("/api/settings/change-password", request);

        var result = await response.Content.ReadFromJsonAsync<ChangePasswordResult>();

        return result ?? new ChangePasswordResult
        {
            Success = false,
            Message = "Something went wrong. Please try again."
        };
    }

    public async Task<UpdateEmailResult> UpdateEmailAsync(UpdateEmailRequest request)
    {
        var response = await _http.PostAsJsonAsync("/api/settings/update-email", request);

        var result = await response.Content.ReadFromJsonAsync<UpdateEmailResult>();

        return result ?? new UpdateEmailResult
        {
            Success = false,
            Message = "Something went wrong. Please try again."
        };
    }

    public async Task<DeleteAccountResult> DeleteAccountAsync(DeleteAccountRequest request)
    {
        var response = await _http.PostAsJsonAsync("/api/settings/delete-account", request);

        var result = await response.Content.ReadFromJsonAsync<DeleteAccountResult>();

        return result ?? new DeleteAccountResult
        {
            Success = false,
            Message = "Something went wrong. Please try again."
        };
    }

   
    

    

}