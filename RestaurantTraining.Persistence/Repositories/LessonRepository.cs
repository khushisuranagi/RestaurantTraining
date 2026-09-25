using Microsoft.EntityFrameworkCore;
using RestaurantTraining.Application.Common.Interfaces;
using RestaurantTraining.Domain.Entities;

namespace RestaurantTraining.Persistence.Repositories
{
    public class LessonRepository : ILessonRepository
    {
        private readonly AppDbContext _context;

        public LessonRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddLessonAsync(
            Lesson lesson,
            CancellationToken cancellationToken)
        {
            await _context.Lessons.AddAsync(
                lesson,
                cancellationToken);

            await _context.SaveChangesAsync(
                cancellationToken);

            return lesson.LessonId;
        }

        public async Task<List<Lesson>> GetAllLessonsAsync(
            CancellationToken cancellationToken)
        {
            return await _context.Lessons
                .ToListAsync(cancellationToken);
        }

        public async Task<Lesson?> GetLessonByIdAsync(
            int lessonId,
            CancellationToken cancellationToken)
        {
            return await _context.Lessons
                .FirstOrDefaultAsync(
                    x => x.LessonId == lessonId,
                    cancellationToken);
        }

        public async Task UpdateLessonAsync(
            Lesson lesson,
            CancellationToken cancellationToken)
        {
            _context.Lessons.Update(lesson);

            await _context.SaveChangesAsync(
                cancellationToken);
        }

        public async Task DeleteLessonAsync(
    Lesson lesson,
    CancellationToken cancellationToken)
        {
            await using var transaction =
                await _context.Database.BeginTransactionAsync(cancellationToken);

            var resourceIds = await _context.LessonResources
                .Where(r => r.LessonId == lesson.LessonId)
                .Select(r => r.ResourceId)
                .ToListAsync(cancellationToken);

            // Per-resource AI content (no FK, but remove orphans)
            await _context.ResourceQuestionAttempts
                .Where(a => resourceIds.Contains(a.ResourceId))
                .ExecuteDeleteAsync(cancellationToken);
            await _context.ResourceQuestionOptions
                .Where(o => _context.ResourceQuestions
                    .Where(q => resourceIds.Contains(q.ResourceId))
                    .Select(q => q.ResourceQuestionId)
                    .Contains(o.ResourceQuestionId))
                .ExecuteDeleteAsync(cancellationToken);
            await _context.ResourceQuestions
                .Where(q => resourceIds.Contains(q.ResourceId))
                .ExecuteDeleteAsync(cancellationToken);
            await _context.ResourceSummaries
                .Where(s => resourceIds.Contains(s.ResourceId))
                .ExecuteDeleteAsync(cancellationToken);

            // Direct children of the lesson
            await _context.LessonProgress
                .Where(p => p.LessonId == lesson.LessonId)
                .ExecuteDeleteAsync(cancellationToken);
            await _context.LessonResources
                .Where(r => r.LessonId == lesson.LessonId)
                .ExecuteDeleteAsync(cancellationToken);

            // The lesson itself
            await _context.Lessons
                .Where(l => l.LessonId == lesson.LessonId)
                .ExecuteDeleteAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);
        }
    }
}