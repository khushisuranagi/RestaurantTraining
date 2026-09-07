namespace RestaurantTraining.Domain.Entities
{
    public class Lesson
    {
        public int LessonId { get; set; }

        public int ModuleId { get; set; }

        public string LessonTitle { get; set; } = string.Empty;

        public int SortOrder { get; set; }

        public bool IsActive { get; set; }

        public string Description { get; set; } = string.Empty;
    }
}