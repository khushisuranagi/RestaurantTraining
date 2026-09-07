using RestaurantTraining.Domain.Enums;

namespace RestaurantTraining.Application.Features.LessonResources.Queries.GetLessonResources
{
    public class LessonResourceDto
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
}