using RestaurantTraining.Blazor.Models;

namespace RestaurantTraining.Blazor.Services.LessonManagement;

public interface ILessonManagementService
{
    Task<LessonSummary?> GetLessonByIdAsync(
        int lessonId);

    Task<List<LessonResourceSummary>> GetResourcesAsync();

    Task<SaveLessonResourceResult> SaveResourceAsync(
        SaveLessonResourceRequest request);

    Task<bool> DeleteResourceAsync(
        int resourceId);
}
