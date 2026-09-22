namespace RestaurantTraining.Domain.Entities
{
    // An AI-generated multiple-choice question for ONE lesson resource.
    // Generated once (on first learner view) and then reused from the database.
    public class ResourceQuestion
    {
        public int ResourceQuestionId { get; set; }

        // The lesson resource this question belongs to (one question per resource).
        public int ResourceId { get; set; }

        public string QuestionText { get; set; } = string.Empty;

        // Short explanation of the correct answer.
        public string Explanation { get; set; } = string.Empty;

        public DateTime GeneratedAt { get; set; }
    }
}
