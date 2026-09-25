using RestaurantTraining.Web.Models;
using System.Net.Http.Json;

namespace RestaurantTraining.Web.Services.ProfileManagement;

public class ProfileService : IProfileService
{
    private readonly HttpClient _http;  //calls api over http
    public ProfileService(HttpClient http) => _http = http;

    public async Task<ProfileModel?> GetProfileAsync()
    {
        var response = await _http.GetAsync("/api/profile");
        return response.IsSuccessStatusCode
            ? await response.Content.ReadFromJsonAsync<ProfileModel>()
            : null;//json
    }

    public async Task<ProfileUpdateResult> UpdateProfileAsync(UpdateProfileRequest request)
    {
        var response = await _http.PutAsJsonAsync("/api/profile", request);

        // The API returns a ProfileUpdateResult for both success and validation errors.
        var result = await response.Content.ReadFromJsonAsync<ProfileUpdateResult>();

        return result ?? new ProfileUpdateResult
        {
            Success = false,
            Message = "Could not update your profile."
        };
    }
}
