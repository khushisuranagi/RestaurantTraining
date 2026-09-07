using RestaurantTraining.Domain.Entities;

namespace RestaurantTraining.Application.Common.Interfaces
{
    public interface IQuizQuestionRepository
    {
        Task<int> AddQuizQuestionAsync(
            QuizQuestion question,
            CancellationToken cancellationToken);

        Task<List<QuizQuestion>> GetAllQuizQuestionsAsync(
            CancellationToken cancellationToken);

        Task<QuizQuestion?> GetQuizQuestionByIdAsync(
            int questionId,
            CancellationToken cancellationToken);

        Task UpdateQuizQuestionAsync(
            QuizQuestion question,
            CancellationToken cancellationToken);

        Task DeleteQuizQuestionAsync(
            QuizQuestion question,
            CancellationToken cancellationToken);
    }
}