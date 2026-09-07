using RestaurantTraining.Blazor.Models;
using System.Net.Http.Json;

namespace RestaurantTraining.Blazor.Services.ProfileManagement;

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
}
