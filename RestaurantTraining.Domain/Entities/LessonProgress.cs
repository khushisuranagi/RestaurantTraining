namespace RestaurantTraining.Domain.Entities
{
    public class LessonProgress
    {
        public int LessonProgressId { get; set; }

        public int UserId { get; set; }

        public int LessonId { get; set; }

        public bool IsCompleted { get; set; }

        public DateTime? CompletedAt { get; set; }
    }
}