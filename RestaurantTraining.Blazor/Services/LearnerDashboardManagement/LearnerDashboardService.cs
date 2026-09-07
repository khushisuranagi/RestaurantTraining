using System.Net.Http.Json;
using RestaurantTraining.Blazor.Models;

namespace RestaurantTraining.Blazor.Services.LearnerDashboardManagement;

public class LearnerDashboardService : ILearnerDashboardService
{
    private readonly HttpClient _http;

    public LearnerDashboardService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<LearnerModuleModel>> GetAssignedModulesAsync()
    {
        return await _http.GetFromJsonAsync<List<LearnerModuleModel>>(
            "/api/learner/modules") ?? [];
    }

    public async Task<LearnerModuleContentModel?> GetAssignedModuleAsync(int moduleId)
    {
        var response = await _http.GetAsync($"/api/learner/modules/{moduleId}");
        return response.IsSuccessStatusCode
            ? await response.Content.ReadFromJsonAsync<LearnerModuleContentModel>()
            : null;
    }

    public async Task<LearnerDashboardModel?> GetDashboardAsync()
    {
        var response = await _http.GetAsync("/api/learner/dashboard");
        return response.IsSuccessStatusCode
            ? await response.Content.ReadFromJsonAsync<LearnerDashboardModel>()
            : null;
    }

    public async Task<bool> MarkLessonCompleteAsync(int lessonId)
    {
        var response = await _http.PostAsync(
            $"/api/learner/modules/lessons/{lessonId}/complete", null);
        return response.IsSuccessStatusCode;
    }

    public async Task<List<LearnerCertificateModel>> GetCertificatesAsync()
    {
        return await _http.GetFromJsonAsync<List<LearnerCertificateModel>>(
            "/api/learner/certificates") ?? [];
    }

    public async Task<List<LearnerModuleOverviewModel>> GetModulesOverviewAsync()
    {
        return await _http.GetFromJsonAsync<List<LearnerModuleOverviewModel>>(
            "/api/learner/my-modules/overview") ?? [];
    }

}
