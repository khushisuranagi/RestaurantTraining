namespace RestaurantTraining.Application.Common.Interfaces
{
    // Data access for the Content Creator dashboard.
  
    public interface IContentCreatorDashboardRepository
    {
        Task<List<DashboardModuleInfo>> GetModulesAsync(CancellationToken cancellationToken);

        Task<List<DashboardLessonInfo>> GetActiveLessonsAsync(CancellationToken cancellationToken);

        Task<List<int>> GetQuizQuestionModuleIdsAsync(CancellationToken cancellationToken);

        Task<List<int>> GetAssignedModuleIdsAsync(CancellationToken cancellationToken);

        Task<List<int>> GetAssignedRoleIdsAsync(CancellationToken cancellationToken);

        Task<List<int>> GetAllUserRoleIdsAsync(CancellationToken cancellationToken);

        Task<List<int>> GetLessonIdsWithResourcesAsync(CancellationToken cancellationToken);

        Task<int> GetCertificateCountAsync(CancellationToken cancellationToken);

        Task<int> GetDistinctCertificateLearnerCountAsync(CancellationToken cancellationToken);

        Task<int> GetActiveLearnerCountAsync(CancellationToken cancellationToken);

        Task<List<DashboardRecentCertificateInfo>> GetRecentCertificatesAsync(
            int take, CancellationToken cancellationToken);
    }

    
    public class DashboardModuleInfo
    {
        public int ModuleId { get; set; }
        public string ModuleName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class DashboardLessonInfo
    {
        public int LessonId { get; set; }
        public string LessonTitle { get; set; } = string.Empty;
        public int ModuleId { get; set; }
    }

    public class DashboardRecentCertificateInfo
    {
        public string LearnerName { get; set; } = string.Empty;
        public string ModuleName { get; set; } = string.Empty;
        public DateTime IssuedDate { get; set; }
    }
}
