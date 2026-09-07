using RestaurantTraining.Blazor.Models;

namespace RestaurantTraining.Blazor.Services.LearnerDashboardManagement;

public interface ILearnerDashboardService
{
    Task<List<LearnerModuleModel>> GetAssignedModulesAsync();
    Task<LearnerModuleContentModel?> GetAssignedModuleAsync(int moduleId);
    Task<bool> MarkLessonCompleteAsync(int lessonId);
    Task<List<LearnerCertificateModel>> GetCertificatesAsync();
    Task<List<LearnerModuleOverviewModel>> GetModulesOverviewAsync();
    Task<LearnerDashboardModel?> GetDashboardAsync();
}
