using System.Net.Http.Json;
using RestaurantTraining.Blazor.Models;

namespace RestaurantTraining.Blazor.Services.LearnerExplore;

public class ExploreService : IExploreService
{
    private readonly HttpClient _http;

    public ExploreService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<ExploreModuleModel>> GetExploreModulesAsync()
    {
        return await _http.GetFromJsonAsync<List<ExploreModuleModel>>(
            "/api/learner/explore") ?? [];
    }

    public async Task<LearnerModuleContentModel?> GetExploreModuleAsync(int moduleId)
    {
        var response = await _http.GetAsync($"/api/learner/explore/{moduleId}");
        return response.IsSuccessStatusCode
            ? await response.Content.ReadFromJsonAsync<LearnerModuleContentModel>()
            : null;
    }
}
