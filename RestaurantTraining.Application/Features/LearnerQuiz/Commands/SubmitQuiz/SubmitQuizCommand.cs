using MediatR;

namespace RestaurantTraining.Application.Features.LearnerQuiz.Commands.SubmitQuiz
{
    public class SubmitQuizCommand : IRequest<SubmitQuizOutcome>
    {
        // Set by the controller from the JWT / route:
        public int UserId { get; set; }
        public int ModuleId { get; set; }
        public string RoleName { get; set; } = string.Empty;

        // Bound from the request body:
        public List<QuizAnswerInput> Answers { get; set; } = [];
    }

    public class QuizAnswerInput
    {
        public int QuestionId { get; set; }
        public List<int> SelectedOptionIds { get; set; } = [];  // MCQ / True-False
        public string? TextAnswer { get; set; }                  // Fill-in-the-Blank
    }
}
