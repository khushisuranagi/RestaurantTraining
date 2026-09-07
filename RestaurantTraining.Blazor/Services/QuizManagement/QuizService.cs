using System.Net.Http.Json;
using RestaurantTraining.Blazor.Models;

namespace RestaurantTraining.Blazor.Services.QuizManagement;

public class QuizService : IQuizService
{
    private readonly HttpClient _http;

    public QuizService(HttpClient http)
    {
        _http = http;
    }


   
    // READ
    

    public async Task<List<QuizModuleSummary>> GetModulesAsync()
    {
        return await _http.GetFromJsonAsync<
            List<QuizModuleSummary>>(
                "/api/Modules") ?? [];
    }


    public async Task<List<QuizQuestionSummary>> GetQuestionsAsync()
    {
        return await _http.GetFromJsonAsync<
            List<QuizQuestionSummary>>(
                "/api/QuizQuestions") ?? [];
    }


    public async Task<List<QuizOptionSummary>> GetOptionsAsync()
    {
        return await _http.GetFromJsonAsync<
            List<QuizOptionSummary>>(
                "/api/QuizOptions") ?? [];
    }


    
    // QUESTIONS: CREATE / UPDATE
    
    
    public async Task<QuizSaveResult> SaveQuestionAsync(
        SaveQuizQuestionRequest request)
    {
        HttpResponseMessage response;

        if (request.QuestionId == 0)
        {
            // Create a new question
            response = await _http.PostAsJsonAsync(
                "/api/QuizQuestions",
                request);
        }
        else
        {
            // Update an existing question by its id
            response = await _http.PutAsJsonAsync(
                $"/api/QuizQuestions/{request.QuestionId}",
                request);
        }

        return await ReadResultAsync(response);
    }


    public async Task<bool> DeleteQuestionAsync(int questionId)
    {
        var response = await _http.DeleteAsync(
            $"/api/QuizQuestions/{questionId}");

        return response.IsSuccessStatusCode;
    }


   
    // OPTIONS: CREATE / UPDATE
    

    public async Task<QuizSaveResult> SaveOptionAsync(
        SaveQuizOptionRequest request)
    {
        HttpResponseMessage response;

        if (request.OptionId == 0)
        {
            // new option
            response = await _http.PostAsJsonAsync(
                "/api/QuizOptions",
                request);
        }
        else
        {
            // Update existing option 
            response = await _http.PutAsJsonAsync(
                $"/api/QuizOptions/{request.OptionId}",
                request);
        }

        return await ReadResultAsync(response);
    }


    public async Task<bool> DeleteOptionAsync(int optionId)
    {
        var response = await _http.DeleteAsync(
            $"/api/QuizOptions/{optionId}");

        return response.IsSuccessStatusCode;
    }


   
    // SHARED HELPER
  

    // turns the api's HTTP response into a simple success/message result.
   
    private static async Task<QuizSaveResult> ReadResultAsync(
        HttpResponseMessage response)
    {
        SaveResponse? body = null;

        try
        {
            body = await response.Content
                .ReadFromJsonAsync<SaveResponse>();
        }
        catch
        {
            // the body might be empty/ not json
        }

        if (response.IsSuccessStatusCode)
        {

            var newId = body == null
                ? 0
                : (body.QuestionId != 0 ? body.QuestionId : body.OptionId);

            return new QuizSaveResult
            {
                Success = true,
                Id = newId
            };
        }

        return new QuizSaveResult
        {
            Success = false,
            Message = string.IsNullOrWhiteSpace(body?.Message)
                ? "Could not save. Please try again."
                : body!.Message
        };
    }


    
    private class SaveResponse
    {
        public bool Success { get; set; }

        public string Message { get; set; } = string.Empty;

        public int QuestionId { get; set; }

        public int OptionId { get; set; }
    }
}
