namespace RestaurantTraining.Application.Features.LearnerExplore.Queries.GetExploreModule
{
    // Matches the Blazor LearnerModuleContentModel (reused on the frontend).
    public class ExploreModuleContentDto
    {
        public int ModuleId { get; set; }
        public string ModuleName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<ExploreLessonDto> Lessons { get; set; } = [];
    }

    public class ExploreLessonDto
    {
        public int LessonId { get; set; }
        public string LessonTitle { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int SortOrder { get; set; }
        public bool IsCompleted { get; set; }
        public List<ExploreResourceDto> Resources { get; set; } = [];
    }

    public class ExploreResourceDto
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
