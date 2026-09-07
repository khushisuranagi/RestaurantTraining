namespace RestaurantTraining.Blazor.Models
{
    
        // Rich overview for the Content Creator dashboard.
        public class ContentCreatorDashboardModel
        {
            public int ModuleCount { get; set; }
            public int ActiveModules { get; set; }
            public int InactiveModules { get; set; }
            public int LessonCount { get; set; }
            public int QuizQuestionCount { get; set; }
            public int CertificatesIssued { get; set; }
            public int ModulesAssignedCount { get; set; }

            public int LearnersAssigned { get; set; }
            public int ActiveLearners { get; set; }
            public int LearnersCompleted { get; set; }
            public List<RecentCertificateItem> RecentCertificates { get; set; } = [];

            public List<NamedModuleItem> ModulesWithoutLessons { get; set; } = [];
            public List<NamedModuleItem> ModulesWithoutQuiz { get; set; } = [];
            public List<NamedModuleItem> ModulesNotAssigned { get; set; } = [];
            public List<NamedLessonItem> LessonsWithoutResources { get; set; } = [];

            public List<RecentModuleItem> RecentModules { get; set; } = [];
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


        // Everything the dashboard needs in one object,
        // so the page can load it with a single call.
        public class DashboardData
        {
            public List<DashboardModuleSummary> Modules { get; set; } = [];

            public List<DashboardLessonSummary> Lessons { get; set; } = [];
        }


        public class DashboardModuleSummary
        {
            public int ModuleId { get; set; }

            public string ModuleName { get; set; } = string.Empty;

            public string Description { get; set; } = string.Empty;

            public bool IsActive { get; set; }

            public DateTime CreatedAt { get; set; }
        }


        public class DashboardLessonSummary
        {
            public int LessonId { get; set; }
        }

    
}
