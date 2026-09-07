using RestaurantTraining.Domain.Enums;

namespace RestaurantTraining.Application.Features.QuizQuestions.Queries.GetQuizQuestions
{
    public class QuizQuestionDto
    {
        public int QuestionId { get; set; }

        public int ModuleId { get; set; }

        public string QuestionText { get; set; } = string.Empty;

        public QuestionType QuestionType { get; set; }

        public string ImageUrl { get; set; } = string.Empty;

        public string Explanation { get; set; } = string.Empty;

        public int Marks { get; set; }
    }
}