using RestaurantTraining.Domain.Enums;

namespace RestaurantTraining.Application.Common.Interfaces
{
    // Data access for a learner taking a quiz.
    public interface ILearnerQuizRepository
    {
        Task<bool> IsAssignedToLearnerAsync(
            string roleName, int moduleId, CancellationToken cancellationToken);

        // Questions + options w/o the correct answers for learner
        Task<List<QuizQuestionInfo>> GetQuizQuestionsAsync(
            int moduleId, CancellationToken cancellationToken);

        // Questions + options w IsCorrect
        Task<List<GradingQuestionInfo>> GetGradingQuestionsAsync(
            int moduleId, CancellationToken cancellationToken);

        
        Task<int?> GetPassingScoreAsync(
            int moduleId, CancellationToken cancellationToken);

        Task<bool> HasCertificateAsync(
            int userId, int moduleId, CancellationToken cancellationToken);

        
        Task RecordAttemptAsync(
            int userId, int moduleId, decimal score, decimal totalMarks,
            bool passed, string? certificateNumber, CancellationToken cancellationToken);

        Task<string> GetModuleNameAsync(
            int moduleId, CancellationToken cancellationToken);
    }

 
    public class QuizQuestionInfo
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; } = string.Empty;
        public int QuestionType { get; set; }
        public int Marks { get; set; }
        public List<QuizOptionInfo> Options { get; set; } = [];
    }

    public class QuizOptionInfo
    {
        public int OptionId { get; set; }
        public string OptionText { get; set; } = string.Empty;
    }


    public class GradingQuestionInfo
    {
        public int QuestionId { get; set; }
        public QuestionType QuestionType { get; set; }
        public int Marks { get; set; }
        public List<GradingOptionInfo> Options { get; set; } = [];
    }

    public class GradingOptionInfo
    {
        public int OptionId { get; set; }
        public string OptionText { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
    }
}
