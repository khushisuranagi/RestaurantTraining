namespace RestaurantTraining.Application.Features.LearnerDashboard.Queries.GetLearnerDashboard
{
    // Matches the Blazor LearnerDashboardModel (field names unchanged).
    public class LearnerDashboardDto
    {
        public int LessonsCompleted { get; set; }
        public int TotalLessons { get; set; }

        public int ModulesCompleted { get; set; }
        public int TotalModules { get; set; }

        public int QuizzesAttempted { get; set; }
        public int TotalQuizzes { get; set; }

        public int CertificatesEarned { get; set; }
        public int CertificatesPending { get; set; }

        public int AiScenariosCompleted { get; set; }

        // Started but not finished yet.
        public List<PendingModuleDto> PendingModules { get; set; } = [];
    }

    public class PendingModuleDto
    {
        public int ModuleId { get; set; }
        public string ModuleName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int CompletedLessons { get; set; }
        public int TotalLessons { get; set; }
        public int ProgressPercent { get; set; }
        public bool IsCompleted { get; set; }
    }
}
