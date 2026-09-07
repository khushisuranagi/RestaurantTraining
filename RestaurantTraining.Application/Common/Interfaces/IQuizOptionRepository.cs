using RestaurantTraining.Domain.Entities;

namespace RestaurantTraining.Application.Common.Interfaces
{
    public interface IQuizOptionRepository
    {
        Task<int> AddQuizOptionAsync(
            QuizOption option,
            CancellationToken cancellationToken);

        Task<List<QuizOption>> GetAllQuizOptionsAsync(
            CancellationToken cancellationToken);

        Task<QuizOption?> GetQuizOptionByIdAsync(
            int optionId,
            CancellationToken cancellationToken);

        Task UpdateQuizOptionAsync(
            QuizOption option,
            CancellationToken cancellationToken);

        Task DeleteQuizOptionAsync(
            QuizOption option,
            CancellationToken cancellationToken);
    }
}