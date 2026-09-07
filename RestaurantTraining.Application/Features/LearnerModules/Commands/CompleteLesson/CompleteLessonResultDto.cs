namespace RestaurantTraining.Application.Features.LearnerModules.Commands.CompleteLesson
{
    public class CompleteLessonResultDto
    {
        // True when the lesson does not exist / is inactive
        // (the controller turns this into a 404 Not Found).
        public bool LessonNotFound { get; set; }

        public int LessonId { get; set; }
        public bool IsCompleted { get; set; }
    }
}
