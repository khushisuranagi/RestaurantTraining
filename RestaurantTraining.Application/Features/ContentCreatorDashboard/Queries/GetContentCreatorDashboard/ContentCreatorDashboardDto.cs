namespace RestaurantTraining.Application.Features.ContentCreatorDashboard.Queries.GetContentCreatorDashboard
{
    public class ContentCreatorDashboardDto
    {
        // Counts 
        public int ModuleCount { get; set; }
        public int ActiveModules { get; set; }
        public int InactiveModules { get; set; }
        public int LessonCount { get; set; }
        public int QuizQuestionCount { get; set; }
        public int CertificatesIssued { get; set; }
        public int ModulesAssignedCount { get; set; }

        // Learner progress 
        public int LearnersAssigned { get; set; }
        public int ActiveLearners { get; set; }
        public int LearnersCompleted { get; set; }
        public List<RecentCertificateItem> RecentCertificates { get; set; } = new();

        // Needs attention
        public List<NamedModuleItem> ModulesWithoutLessons { get; set; } = new();
        public List<NamedModuleItem> ModulesWithoutQuiz { get; set; } = new();
        public List<NamedModuleItem> ModulesNotAssigned { get; set; } = new();
        public List<NamedLessonItem> LessonsWithoutResources { get; set; } = new();

        // Recent modules 
        public List<RecentModuleItem> RecentModules { get; set; } = new();
    }

    public class RecentCertificateItem
    {
        public string LearnerName { get; set; } = string.Empty;
        public string ModuleName { get; set; } = string.Empty;
        public DateTime IssuedDate { get; set; }
    }

    public class NamedModuleItem
    {
        public int ModuleId { get; set; }
        public string ModuleName { get; set; } = string.Empty;
    }

    public class NamedLessonItem
    {
        public int LessonId { get; set; }
        public string LessonTitle { get; set; } = string.Empty;
    }

    public class RecentModuleItem
    {
        public int ModuleId { get; set; }
        public string ModuleName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
