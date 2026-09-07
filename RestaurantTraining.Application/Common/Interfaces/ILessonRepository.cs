using RestaurantTraining.Domain.Entities;

namespace RestaurantTraining.Application.Common.Interfaces
{
    public interface ILessonRepository
    {
        Task<int> AddLessonAsync(
            Lesson lesson,
            CancellationToken cancellationToken);

        Task<List<Lesson>> GetAllLessonsAsync(
            CancellationToken cancellationToken);

        Task<Lesson?> GetLessonByIdAsync(
            int lessonId,
            CancellationToken cancellationToken);

        Task UpdateLessonAsync(
            Lesson lesson,
            CancellationToken cancellationToken);

        Task DeleteLessonAsync(
            Lesson lesson,
            CancellationToken cancellationToken);
    }
}