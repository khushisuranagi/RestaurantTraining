namespace RestaurantTraining.Application.Features.LearnerQuiz.Commands.SubmitQuiz
{
    // Lets the controller pick the right HTTP status without the Application
    // layer knowing about HTTP.
    public class SubmitQuizOutcome
    {
        public bool NotAssigned { get; set; }   // → 404 Not Found
        public bool NoQuiz { get; set; }         // → 400 Bad Request
        public QuizResultDto? Result { get; set; }
    }
}
