using RestaurantTraining.Domain.Enums;

namespace RestaurantTraining.Blazor.Models;

public class LessonSummary
{
    public int LessonId { get; set; }
    public int ModuleId { get; set; }
    public string LessonTitle { get; set; } = string.Empty;
    public int SortOrder { get; set; }
    public bool IsActive { get; set; }
    public string Description { get; set; } = string.Empty;
}

public class LessonResourceSummary
{
    public int ResourceId { get; set; }
    public int LessonId { get; set; }
    public ResourceType ResourceType { get; set; }
    public string ResourceUrl { get; set; } = string.Empty;
    public string? FileData { get; set; }
    public string? FileName { get; set; }
    public string? ContentType { get; set; }
    public string ContentText { get; set; } = string.Empty;
    public int SortOrder { get; set; }
    public bool IsActive { get; set; }
}

public class SaveLessonResourceRequest
{
    public int ResourceId { get; set; }
    public int LessonId { get; set; }
    public ResourceType ResourceType { get; set; }
    public string ResourceUrl { get; set; } = string.Empty;
    public string? FileData { get; set; }
    public string? FileName { get; set; }
    public string? ContentType { get; set; }
    public string ContentText { get; set; } = string.Empty;
    public int SortOrder { get; set; }
    public bool IsActive { get; set; }
}

public class SaveLessonResourceResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
}
