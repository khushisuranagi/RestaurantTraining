using Microsoft.EntityFrameworkCore;
using RestaurantTraining.Application.Common.Interfaces;
using RestaurantTraining.Domain.Entities;

namespace RestaurantTraining.Persistence.Repositories
{
    public class ResourceQuestionRepository : IResourceQuestionRepository
    {
        private readonly AppDbContext _context;

        public ResourceQuestionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResourceContextInfo?> GetResourceContextAsync(
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
                    resource.FileName
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (data is null)
                return null;

            return new ResourceContextInfo
            {
                LessonTitle = data.LessonTitle,
                LessonDescription = data.LessonDescription,
                ResourceType = data.ResourceType.ToString(),
                ResourceUrl = data.ResourceUrl,
                ContentText = data.ContentText,
                FileName = data.FileName ?? string.Empty
            };
        }

        public async Task<StoredResourceQuestion?> GetByResourceIdAsync(
            int resourceId, CancellationToken cancellationToken)
        {
            var question = await _context.ResourceQuestions
                .FirstOrDefaultAsync(q => q.ResourceId == resourceId, cancellationToken);

            if (question is null)
                return null;

            var options = await _context.ResourceQuestionOptions
                .Where(o => o.ResourceQuestionId == question.ResourceQuestionId)
                .OrderBy(o => o.OptionId)
                .Select(o => new StoredResourceOption
                {
                    OptionId = o.OptionId,
                    OptionText = o.OptionText,
                    IsCorrect = o.IsCorrect
                })
                .ToListAsync(cancellationToken);

            return new StoredResourceQuestion
            {
                QuestionId = question.ResourceQuestionId,
                QuestionText = question.QuestionText,
                Explanation = question.Explanation,
                Options = options
            };
        }

        public async Task<StoredResourceQuestion> SaveGeneratedAsync(
            int resourceId, GeneratedMcq mcq, CancellationToken cancellationToken)
        {
            var question = new ResourceQuestion
            {
                ResourceId = resourceId,
                QuestionText = mcq.Question,
                Explanation = mcq.Explanation,
                GeneratedAt = DateTime.UtcNow
            };

            _context.ResourceQuestions.Add(question);
            await _context.SaveChangesAsync(cancellationToken);  // gets the question id

            var options = mcq.Options
                .Select(o => new ResourceQuestionOption
                {
                    ResourceQuestionId = question.ResourceQuestionId,
                    OptionText = o.Text,
                    IsCorrect = o.IsCorrect
                })
                .ToList();

            _context.ResourceQuestionOptions.AddRange(options);
            await _context.SaveChangesAsync(cancellationToken);  // gets the option ids

            return new StoredResourceQuestion
            {
                QuestionId = question.ResourceQuestionId,
                QuestionText = question.QuestionText,
                Explanation = question.Explanation,
                Options = options
                    .Select(o => new StoredResourceOption
                    {
                        OptionId = o.OptionId,
                        OptionText = o.OptionText,
                        IsCorrect = o.IsCorrect
                    })
                    .ToList()
            };
        }

        public async Task<bool> HasAnsweredCorrectlyAsync(
            int userId, int resourceId, CancellationToken cancellationToken)
        {
            return await _context.ResourceQuestionAttempts
                .AnyAsync(a => a.UserId == userId && a.ResourceId == resourceId, cancellationToken);
        }

        public async Task<int> RecordCorrectAnswerAsync(
            int userId, int resourceId, int points, CancellationToken cancellationToken)
        {
            // Award points only the first time.
            var already = await _context.ResourceQuestionAttempts
                .AnyAsync(a => a.UserId == userId && a.ResourceId == resourceId, cancellationToken);

            if (already)
                return 0;

            _context.ResourceQuestionAttempts.Add(new ResourceQuestionAttempt
            {
                UserId = userId,
                ResourceId = resourceId,
                PointsAwarded = points,
                AnsweredAt = DateTime.UtcNow
            });

            await _context.SaveChangesAsync(cancellationToken);
            return points;
        }

        public async Task<int> GetTotalPointsAsync(int userId, CancellationToken cancellationToken)
        {
            return await _context.ResourceQuestionAttempts
                .Where(a => a.UserId == userId)
                .SumAsync(a => (int?)a.PointsAwarded, cancellationToken) ?? 0;
        }
    }
}
