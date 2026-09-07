using System.Net.Http.Json;
using RestaurantTraining.Blazor.Models;

namespace RestaurantTraining.Blazor.Services.LessonManagement;

public class LessonManagementService : ILessonManagementService
{
    private readonly HttpClient _http;

    public LessonManagementService(HttpClient http)
    {
        _http = http;
    }


    // GET LESSON
    
    public async Task<LessonSummary?> GetLessonByIdAsync(
        int lessonId)
    {
        var lessons =
            await _http.GetFromJsonAsync<List<LessonSummary>>(
                "/api/Lessons") ?? [];

        return lessons.FirstOrDefault(
            x => x.LessonId == lessonId);
    }


  
    // GET RESOURCES
 

    public async Task<List<LessonResourceSummary>>
        GetResourcesAsync()
    {
        return
            await _http.GetFromJsonAsync<
                List<LessonResourceSummary>>(
                    "/api/LessonResources") ?? [];
    }


    
    // CREATE / UPDATE RESOURCE
  

    public async Task<SaveLessonResourceResult>
        SaveResourceAsync(
            SaveLessonResourceRequest request)
    {
        HttpResponseMessage response;

        if (request.ResourceId == 0)
        {
            response =
                await _http.PostAsJsonAsync(
                    "/api/LessonResources",
                    request);
        }
        else
        {
            response =
                await _http.PutAsJsonAsync(
                    $"/api/LessonResources/{request.ResourceId}",
                    request);
        }

        if (response.IsSuccessStatusCode)
        {
            return new SaveLessonResourceResult
            {
                Success = true,
                Message = "Resource saved successfully."
            };
        }

        try
        {
            var result =
                await response.Content
                    .ReadFromJsonAsync<SaveLessonResourceResult>();

            return result
                ?? new SaveLessonResourceResult
                {
                    Success = false,
                    Message = "Could not save the resource."
                };
        }
        catch
        {
            return new SaveLessonResourceResult
            {
                Success = false,
                Message = "Could not save the resource."
            };
        }
    }


    // DELETE RESOURCE
   

    public async Task<bool> DeleteResourceAsync(
        int resourceId)
    {
        var response =
            await _http.DeleteAsync(
                $"/api/LessonResources/{resourceId}");

        return response.IsSuccessStatusCode;
    }
}