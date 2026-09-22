namespace RestaurantTraining.Domain.Entities
{
    // One answer choice for a ResourceQuestion (an MCQ has four of these).
    public class ResourceQuestionOption
    {
        public int OptionId { get; set; }

        public int ResourceQuestionId { get; set; }

        public string OptionText { get; set; } = string.Empty;

        public bool IsCorrect { get; set; }
    }
}
