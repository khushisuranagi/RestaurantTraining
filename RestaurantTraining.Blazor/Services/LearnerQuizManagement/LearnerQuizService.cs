using System.Net.Http.Json;
using RestaurantTraining.Blazor.Models;

namespace RestaurantTraining.Blazor.Services.LearnerQuizManagement;

public class LearnerQuizService : ILearnerQuizService
{
    private readonly HttpClient _http;
    public LearnerQuizService(HttpClient http) => _http = http;

    public async Task<LearnerQuizModel?> GetQuizAsync(int moduleId)
    {
        var response = await _http.GetAsync($"/api/learner/quiz/{moduleId}");
        return response.IsSuccessStatusCode
            ? await response.Content.ReadFromJsonAsync<LearnerQuizModel>()
            : null;
    }

    public async Task<QuizResultModel?> SubmitQuizAsync(int moduleId, QuizSubmitRequest request)
    {
        var response = await _http.PostAsJsonAsync($"/api/learner/quiz/{moduleId}/submit", request);
        return response.IsSuccessStatusCode
            ? await response.Content.ReadFromJsonAsync<QuizResultModel>()
            : null;
    }
}
