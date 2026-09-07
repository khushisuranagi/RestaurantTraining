using RestaurantTraining.Domain.Enums;

namespace RestaurantTraining.Domain.Entities
{
    public class LessonResource
    {
        public int ResourceId { get; set; }

        public int LessonId { get; set; }

        public ResourceType ResourceType { get; set; }

        // Used for yt videos/articles.
        public string ResourceUrl { get; set; } = string.Empty;

        // Stores uploaded Image/PDF as a Base64 string.
        public string? FileData { get; set; }

        // Original file name.
        public string? FileName { get; set; }

        public string? ContentType { get; set; }

        public string ContentText { get; set; } = string.Empty;

        public int SortOrder { get; set; }

        public bool IsActive { get; set; }
    }
}