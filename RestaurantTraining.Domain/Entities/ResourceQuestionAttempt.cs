namespace RestaurantTraining.Domain.Entities
{
    // Records that a learner answered a resource's MCQ correctly.
    // One row per (user, resource) — its existence means "passed", and it
    // stores the points awarded (so points are given only once).
    public class ResourceQuestionAttempt
    {
        public int AttemptId { get; set; }

        public int UserId { get; set; }

        public int ResourceId { get; set; }

        public int PointsAwarded { get; set; }

        public DateTime AnsweredAt { get; set; }
    }
}
