using RestaurantTraining.Web.Models;

namespace RestaurantTraining.Web.Services.LearnerDashboardManagement;

public interface ILearnerDashboardService
{
    Task<List<LearnerModuleModel>> GetAssignedModulesAsync();
    Task<LearnerModuleContentModel?> GetAssignedModuleAsync(int moduleId);
    Task<bool> MarkLessonCompleteAsync(int lessonId);
    Task<List<LearnerCertificateModel>> GetCertificatesAsync();
    Task<List<LearnerModuleOverviewModel>> GetModulesOverviewAsync();
    Task<LearnerDashboardModel?> GetDashboardAsync();
    Task<ArticlePreviewModel?> GetArticlePreviewAsync(string url);
    Task<byte[]?> DownloadCertificatePdfAsync(int certificateId);
    Task RecordModuleOpenedAsync(int moduleId);

    // AI MCQ for a lesson resource.
    Task<ResourceQuestionModel?> GetResourceQuestionAsync(int resourceId);
    Task<ResourceAnswerResultModel?> SubmitResourceAnswerAsync(int resourceId, int selectedOptionId);

    // AI summary of a lesson resource's content.
    Task<string?> GetResourceSummaryAsync(int resourceId);

    // AI practice scenario.
    Task<ScenarioModel?> GetScenarioAsync(int moduleId);
    Task<ScenarioReplyResult?> ReplyToScenarioAsync(int scenarioId, List<ScenarioChatMessage> messages);
}
