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
            _context.Lessons.Remove(lesson);

            await _context.SaveChangesAsync(
                cancellationToken);
        }
    }
}