using MediatR;
using RestaurantTraining.Application.Common.Interfaces;
using RestaurantTraining.Domain.Enums;

namespace RestaurantTraining.Application.Features.LearnerQuiz.Commands.SubmitQuiz
{
    public class SubmitQuizCommandHandler
        : IRequestHandler<SubmitQuizCommand, SubmitQuizOutcome>
    {
        private readonly ILearnerQuizRepository _repository;

        public SubmitQuizCommandHandler(
            ILearnerQuizRepository repository)
        {
            _repository = repository;
        }

        public async Task<SubmitQuizOutcome> Handle(
            SubmitQuizCommand request,
            CancellationToken cancellationToken)
        {
            var isAssigned = await _repository.IsAssignedToLearnerAsync(
                request.RoleName, request.UserId, request.ModuleId, cancellationToken);

            if (!isAssigned)
                return new SubmitQuizOutcome { NotAssigned = true };

            // Load questions + options (with IsCorrect) for grading only.
            var questions = await _repository.GetGradingQuestionsAsync(
                request.ModuleId, cancellationToken);

            if (questions.Count == 0)
                return new SubmitQuizOutcome { NoQuiz = true };

            var answers = request.Answers.ToDictionary(a => a.QuestionId);

            decimal totalMarks = 0, score = 0;
            int correctCount = 0;


            var review = new List<QuizAnswerReviewDto>();
            foreach (var q in questions)
            {
                totalMarks += q.Marks;
                answers.TryGetValue(q.QuestionId, out var ans);

                string correctAnswer;
                bool isCorrect;
                if (q.QuestionType == QuestionType.FillInTheBlank)
                {
                    correctAnswer = q.Options.FirstOrDefault(o => o.IsCorrect)?.OptionText ?? "";
                    var given = ans?.TextAnswer?.Trim() ?? "";
                    isCorrect = given.Length > 0 &&
                                string.Equals(given, correctAnswer.Trim(), StringComparison.OrdinalIgnoreCase);
                }
                else // MCQ or TrueFalse: selected set must exactly match the correct set
                {
                    var correct = q.Options.Where(o => o.IsCorrect).Select(o => o.OptionId).ToHashSet();
                    var selected = (ans?.SelectedOptionIds ?? new()).ToHashSet();
                    isCorrect = selected.Count > 0 && selected.SetEquals(correct);
                    correctAnswer = string.Join(", ",
                        q.Options.Where(o => o.IsCorrect).Select(o => o.OptionText));
                }

                if (isCorrect) { score += q.Marks; correctCount++; }

                review.Add(new QuizAnswerReviewDto
                {
                    QuestionText = q.QuestionText,
                    IsCorrect = isCorrect,
                    CorrectAnswer = correctAnswer,
                    Explanation = q.Explanation
                });
            }

            var percentage = totalMarks > 0 ? Math.Round(score / totalMarks * 100, 0) : 0;

            // Default to 70 if the module has no configured passing score.
            var passingScore = await _repository.GetPassingScoreAsync(
                request.ModuleId, cancellationToken) ?? 70;

            var passed = percentage >= passingScore;

            // The quiz no longer issues the certificate on its own — the learner
            // must also pass the AI practice scenario. So we just record the attempt;
            // the scenario step issues the certificate once BOTH are passed.
            await _repository.RecordAttemptAsync(
                request.UserId, request.ModuleId, score, totalMarks,
                passed, certificateNumber: null, cancellationToken);

            var moduleName = await _repository.GetModuleNameAsync(
                request.ModuleId, cancellationToken);

            return new SubmitQuizOutcome
            {
                Result = new QuizResultDto
                {
                    Score = score,
                    TotalMarks = totalMarks,
                    Percentage = percentage,
                    PassingScore = passingScore,
                    Passed = passed,
                    CorrectCount = correctCount,
                    TotalQuestions = questions.Count,
                    CertificateIssued = false,   // issued after the practice scenario
                    CertificateNumber = null,
                    ModuleName = moduleName,
                    Review = passed ? review : []
                }
            };
        }
    }
}
