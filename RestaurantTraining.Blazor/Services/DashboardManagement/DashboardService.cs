using RestaurantTraining.Blazor.Models;
using System.Net.Http.Json;

namespace RestaurantTraining.Blazor.Services.DashboardManagement;

public class DashboardService : IDashboardService
{
    private readonly HttpClient _http;

    public DashboardService(HttpClient http)
    {
        _http = http;
    }


    // fetches the modules and lessons the dashboard needs and returns them together in one DashboardData obj
    public async Task<DashboardData> GetDashboardDataAsync()
    {
        var modules =
            await _http.GetFromJsonAsync<List<DashboardModuleSummary>>(
                "/api/Modules") ?? [];

        var lessons =
            await _http.GetFromJsonAsync<List<DashboardLessonSummary>>(
                "/api/Lessons") ?? [];

        return new DashboardData
        {
            Modules = modules,
            Lessons = lessons
        };
    }

    // counts, impact, needs-attention in one call.
    public async Task<ContentCreatorDashboardModel?> GetContentCreatorDashboardAsync()
    {
        var response = await _http.GetAsync("/api/content-creator/dashboard");
        return response.IsSuccessStatusCode
            ? await response.Content.ReadFromJsonAsync<ContentCreatorDashboardModel>()
            : null;
    }
}
