namespace RestaurantTraining.Application.Features.LearnerModules.Queries.GetAssignedModule
{
    // Matches the Blazor LearnerModuleContentModel / LearnerLessonModel /
    // LearnerLessonResourceModel (field names unchanged).
    public class ModuleContentDto
    {
        public int ModuleId { get; set; }
        public string ModuleName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<LessonDto> Lessons { get; set; } = [];
    }

    public class LessonDto
    {
        public int LessonId { get; set; }
        public string LessonTitle { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int SortOrder { get; set; }
        public bool IsCompleted { get; set; }
        public List<LessonResourceDto> Resources { get; set; } = [];
    }

    public class LessonResourceDto
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
}
