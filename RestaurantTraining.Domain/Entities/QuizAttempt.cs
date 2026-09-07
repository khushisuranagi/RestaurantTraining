namespace RestaurantTraining.Domain.Entities
{
    public class QuizAttempt
    {
        public int AttemptId { get; set; }

        public int UserId { get; set; }

        public int ModuleId { get; set; }

        public decimal Score { get; set; }

        public decimal TotalMarks { get; set; }

        public bool Passed { get; set; }

        public DateTime AttemptedAt { get; set; }
    }
}