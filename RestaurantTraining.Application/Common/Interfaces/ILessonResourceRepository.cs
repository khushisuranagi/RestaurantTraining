using RestaurantTraining.Domain.Entities;

namespace RestaurantTraining.Application.Common.Interfaces
{
    public interface ILessonResourceRepository
    {
        Task<int> AddLessonResourceAsync(
            LessonResource resource,
            CancellationToken cancellationToken);

        // Returns true if the given lesson already has at least one resource.
        // Used to enforce the "one resource per lesson" rule.
        Task<bool> LessonHasResourceAsync(
            int lessonId,
            CancellationToken cancellationToken);

        Task<List<LessonResource>> GetAllLessonResourcesAsync(
            CancellationToken cancellationToken);

        Task<LessonResource?> GetLessonResourceByIdAsync(
            int resourceId,
            CancellationToken cancellationToken);

        Task UpdateLessonResourceAsync(
            LessonResource resource,
            CancellationToken cancellationToken);

        Task DeleteLessonResourceAsync(
            LessonResource resource,
            CancellationToken cancellationToken);
    }
}