namespace RestaurantTraining.Blazor.Models;

public class LearnerModuleModel
{
    public int ModuleId { get; set; }

    public string ModuleName { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}

public class LearnerModuleContentModel
{
    public int ModuleId { get; set; }
    public string ModuleName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<LearnerLessonModel> Lessons { get; set; } = [];
}

public class LearnerLessonModel
{
    public int LessonId { get; set; }
    public string LessonTitle { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int SortOrder { get; set; }
    public bool IsCompleted { get; set; }
    public List<LearnerLessonResourceModel> Resources { get; set; } = [];
}

public class LearnerLessonResourceModel
{
    public int ResourceId { get; set; }
    public int ResourceType { get; set; }
    public string ResourceUrl { get; set; } = string.Empty;
    public string? FileData { get; set; }
    public string? FileName { get; set; }
    public string? ContentType { get; set; }
    public string ContentText { get; set; } = string.Empty;
    public int SortOrder { get; set; }
}

public class LearnerDashboardModel
{
    public int LessonsCompleted { get; set; }
    public int ModulesCompleted { get; set; }
    public int TotalModules { get; set; }
    public int TotalLessons { get; set; }
    public int QuizzesAttempted { get; set; }
    public int TotalQuizzes { get; set; }
    public int CertificatesEarned { get; set; }
    public int CertificatesPending { get; set; }
    public int AiScenariosCompleted { get; set; }
    public List<LearnerDashboardModuleModel> PendingModules { get; set; } = [];
}

public class LearnerDashboardModuleModel
{
    public int ModuleId { get; set; }
    public string ModuleName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int CompletedLessons { get; set; }
    public int TotalLessons { get; set; }
    public int ProgressPercent { get; set; }
    public bool IsCompleted { get; set; }
}

// One assigned module + the learner's state (for the Learning Modules page).
public class LearnerModuleOverviewModel
{
    public int ModuleId { get; set; }
    public string ModuleName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool HasStarted { get; set; }
    public bool IsCompleted { get; set; }
    public int CompletedLessons { get; set; }
    public int TotalLessons { get; set; }
    public int ProgressPercent { get; set; }
}