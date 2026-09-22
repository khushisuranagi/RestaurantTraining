using Microsoft.EntityFrameworkCore;
using RestaurantTraining.Application.Common.Interfaces;
using RestaurantTraining.Domain.Entities;

namespace RestaurantTraining.Persistence.Repositories
{
    public class ResourceSummaryRepository : IResourceSummaryRepository
    {
        private readonly AppDbContext _context;

        public ResourceSummaryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<string?> GetSummaryAsync(
            int resourceId, CancellationToken cancellationToken)
        {
            return await _context.ResourceSummaries
                .Where(s => s.ResourceId == resourceId)
                .Select(s => s.SummaryText)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task SaveSummaryAsync(
            int resourceId, string summaryText, CancellationToken cancellationToken)
        {
            // Guard against a duplicate if two requests race.
            var exists = await _context.ResourceSummaries
                .AnyAsync(s => s.ResourceId == resourceId, cancellationToken);

            if (exists)
                return;

            _context.ResourceSummaries.Add(new ResourceSummary
            {
                ResourceId = resourceId,
                SummaryText = summaryText,
                GeneratedAt = DateTime.UtcNow
            });

            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<ResourceMediaContext?> GetMediaContextAsync(
            int resourceId, CancellationToken cancellationToken)
        {
            var data = await (
                from resource in _context.LessonResources
                join lesson in _context.Lessons on resource.LessonId equals lesson.LessonId
                where resource.ResourceId == resourceId && resource.IsActive
                select new
                {
                    lesson.LessonTitle,
                    LessonDescription = lesson.Description,
                    resource.ResourceType,
                    resource.ResourceUrl,
                    resource.ContentText,
                    resource.FileName,
                    resource.FileData,
                    resource.ContentType
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (data is null)
                return null;

            return new ResourceMediaContext
            {
                LessonTitle = data.LessonTitle,
                LessonDescription = data.LessonDescription,
                ResourceType = data.ResourceType.ToString(),
                ResourceUrl = data.ResourceUrl,
                ContentText = data.ContentText,
                FileName = data.FileName ?? string.Empty,
                FileData = data.FileData,
                ContentType = data.ContentType
            };
        }
    }
}
