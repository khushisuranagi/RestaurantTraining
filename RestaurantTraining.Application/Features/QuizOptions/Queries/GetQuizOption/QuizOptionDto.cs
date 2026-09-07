namespace RestaurantTraining.Application.Features.QuizOptions.Queries.GetQuizOptions
{
    public class QuizOptionDto
    {
        public int OptionId { get; set; }

        public int QuestionId { get; set; }

        public string OptionText { get; set; } = string.Empty;

        public bool IsCorrect { get; set; }
    }
}