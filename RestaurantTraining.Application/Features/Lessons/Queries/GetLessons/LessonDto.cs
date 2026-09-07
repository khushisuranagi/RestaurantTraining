namespace RestaurantTraining.Application.Features.Lessons.Queries.GetLessons
{
    public class LessonDto
    {
        public int LessonId { get; set; }

        public int ModuleId { get; set; }

        public string LessonTitle { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int SortOrder { get; set; }

        public bool IsActive { get; set; }
    }
}