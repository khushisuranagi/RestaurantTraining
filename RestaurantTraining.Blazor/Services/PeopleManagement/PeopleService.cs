using System.Net.Http.Json;
using RestaurantTraining.Web.Models;

namespace RestaurantTraining.Web.Services.PeopleManagement;

public class PeopleService : IPeopleService
{
    private readonly HttpClient _http;
    public PeopleService(HttpClient http) => _http = http;

    public async Task<List<PeopleRoleGroup>> GetPeopleAsync()
    {
        return await _http.GetFromJsonAsync<List<PeopleRoleGroup>>(
            "/api/content-creator/people") ?? [];
    }

    public async Task<LearnerProfileModel?> GetLearnerProfileAsync(int userId)
    {
        var response = await _http.GetAsync($"/api/content-creator/people/{userId}");
        return response.IsSuccessStatusCode
            ? await response.Content.ReadFromJsonAsync<LearnerProfileModel>()
            : null;
    }

    public async Task<SetLearnerActiveStatusResponse?> SetLearnerActiveStatusAsync(int userId, bool isActive)
    {
        var response = await _http.PatchAsJsonAsync(
            $"/api/content-creator/people/{userId}/status",
            new SetLearnerActiveStatusRequest { IsActive = isActive });

        return response.IsSuccessStatusCode
            ? await response.Content.ReadFromJsonAsync<SetLearnerActiveStatusResponse>()
            : null;
    }
}