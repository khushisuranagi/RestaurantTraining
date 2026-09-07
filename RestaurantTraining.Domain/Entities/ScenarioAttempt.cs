namespace RestaurantTraining.Domain.Entities
{
    public class ScenarioAttempt
    {
        public int AttemptId { get; set; }

        public int UserId { get; set; }

        public int ScenarioId { get; set; }

        public decimal Score { get; set; }

        public bool Passed { get; set; }

        public string Feedback { get; set; } = string.Empty;

        public DateTime AttemptedAt { get; set; }
    }
}