using RestaurantTraining.Web.Models;

namespace RestaurantTraining.Web.Services.LessonManagement;

public interface ILessonManagementService
{
    Task<LessonSummary?> GetLessonByIdAsync(
        int lessonId);

    Task<List<LessonResourceSummary>> GetResourcesAsync();

    Task<SaveLessonResourceResult> SaveResourceAsync(
        SaveLessonResourceRequest request);

    Task<bool> DeleteResourceAsync(
        int resourceId);

    Task<ArticlePreviewModel?> GetArticlePreviewAsync(string url);
}
