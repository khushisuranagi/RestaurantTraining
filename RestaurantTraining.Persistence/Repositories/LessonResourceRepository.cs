using Microsoft.EntityFrameworkCore;
using RestaurantTraining.Application.Common.Interfaces;
using RestaurantTraining.Domain.Entities;

namespace RestaurantTraining.Persistence.Repositories
{
    public class LessonResourceRepository : ILessonResourceRepository
    {
        private readonly AppDbContext _context;

        public LessonResourceRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddLessonResourceAsync(
            LessonResource resource,
            CancellationToken cancellationToken)
        {
            await _context.LessonResources.AddAsync(
                resource,
                cancellationToken);

            await _context.SaveChangesAsync(
                cancellationToken);

            return resource.ResourceId;
        }

        public async Task<bool> LessonHasResourceAsync(
            int lessonId,
            CancellationToken cancellationToken)
        {
            // AnyAsync asks the database: "is there at least one row
            // in LessonResources for this LessonId?" It returns true/false
            // without loading the actual rows, so it is fast and simple.
            return await _context.LessonResources
                .AnyAsync(
                    x => x.LessonId == lessonId,
                    cancellationToken);
        }

        public async Task<List<LessonResource>> GetAllLessonResourcesAsync(
            CancellationToken cancellationToken)
        {
            return await _context.LessonResources
                .ToListAsync(cancellationToken);
        }

        public async Task<LessonResource?> GetLessonResourceByIdAsync(
            int resourceId,
            CancellationToken cancellationToken)
        {
            return await _context.LessonResources
                .FirstOrDefaultAsync(
                    x => x.ResourceId == resourceId,
                    cancellationToken);
        }

        public async Task UpdateLessonResourceAsync(
            LessonResource resource,
            CancellationToken cancellationToken)
        {
            _context.LessonResources.Update(resource);

            await _context.SaveChangesAsync(
                cancellationToken);
        }

        public async Task DeleteLessonResourceAsync(
            LessonResource resource,
            CancellationToken cancellationToken)
        {
            _context.LessonResources.Remove(resource);

            await _context.SaveChangesAsync(
                cancellationToken);
        }
    }
}