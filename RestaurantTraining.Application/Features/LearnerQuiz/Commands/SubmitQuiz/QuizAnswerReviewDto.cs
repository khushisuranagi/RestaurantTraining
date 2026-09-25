namespace RestaurantTraining.Application.Features.LearnerQuiz.Commands.SubmitQuiz
{
    // One reviewed question, shown only after the learner passes.
    public class QuizAnswerReviewDto
    {
        public string QuestionText { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }           // did the learner get it right?
        public string CorrectAnswer { get; set; } = string.Empty;  // the correct option(s) text
        public string Explanation { get; set; } = string.Empty;
    }
}