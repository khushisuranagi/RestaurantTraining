namespace RestaurantTraining.Application.Features.ResourceQuestions.Commands.GetOrGenerateResourceQuestion
{
    // What the learner sees: the question and its options WITHOUT which is correct.
    public class ResourceQuestionDto
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; } = string.Empty;
        public List<ResourceQuestionOptionDto> Options { get; set; } = [];

        // True if this learner has already answered it correctly (gate stays open).
        public bool AlreadyAnsweredCorrectly { get; set; }
    }

    public class ResourceQuestionOptionDto
    {
        public int OptionId { get; set; }
        public string OptionText { get; set; } = string.Empty;
    }

    // Wrapper so the controller can return 404 when no question can be produced.
    public class ResourceQuestionResult
    {
        public bool NotAvailable { get; set; }
        public ResourceQuestionDto? Question { get; set; }
    }
}
