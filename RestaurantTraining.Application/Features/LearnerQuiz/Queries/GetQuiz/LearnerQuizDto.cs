namespace RestaurantTraining.Application.Features.LearnerQuiz.Queries.GetQuiz
{
    // Matches the Blazor LearnerQuizModel / LearnerQuizQuestion / LearnerQuizOption.
    // Deliberately has NO correct-answer field.
    public class LearnerQuizDto
    {
        public int ModuleId { get; set; }
        public List<LearnerQuizQuestionDto> Questions { get; set; } = [];
    }

    public class LearnerQuizQuestionDto
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; } = string.Empty;
        public int QuestionType { get; set; }
        public int Marks { get; set; }
        public List<LearnerQuizOptionDto> Options { get; set; } = [];
    }

    public class LearnerQuizOptionDto
    {
        public int OptionId { get; set; }
        public string OptionText { get; set; } = string.Empty;
    }
}
