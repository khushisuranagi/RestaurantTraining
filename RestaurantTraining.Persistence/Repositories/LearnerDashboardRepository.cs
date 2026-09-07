using Microsoft.EntityFrameworkCore;
using RestaurantTraining.Application.Common.Interfaces;
using RestaurantTraining.Domain.Entities;

namespace RestaurantTraining.Persistence.Repositories
{
    public class LearnerDashboardRepository : ILearnerDashboardRepository
    {
        private readonly AppDbContext _context;

        public LearnerDashboardRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<LearnerModuleStateInfo>> GetLearnerModuleStatesAsync(
            int userId, CancellationToken cancellationToken)
        {
            var assignedModules = await (
                from roleModule in _context.RoleModules
                join user in _context.Users
                    on roleModule.RoleId equals user.RoleId
                join module in _context.Modules
                    on roleModule.ModuleId equals module.ModuleId
                where user.UserId == userId && module.IsActive
                select new
                {
                    module.ModuleId,
                    module.ModuleName,
                    module.Description,
                    roleModule.AssignedAt
                }
            ).ToListAsync(cancellationToken);

            var moduleIds = assignedModules
                .Select(x => x.ModuleId)
                .ToList();

            var lessons = await _context.Lessons
                .Where(x => moduleIds.Contains(x.ModuleId) && x.IsActive)
                .Select(x => new { x.LessonId, x.ModuleId })
                .ToListAsync(cancellationToken);

            var completedLessonIdSet = (await _context.LessonProgress
                .Where(x => x.UserId == userId && x.IsCompleted)
                .Select(x => x.LessonId)
                .ToListAsync(cancellationToken))
                .ToHashSet();

            var passedModuleIdSet = (await _context.QuizAttempts
                .Where(x => x.UserId == userId && x.Passed)
                .Select(x => x.ModuleId)
                .Distinct()
                .ToListAsync(cancellationToken))
                .ToHashSet();

            var progressByModuleId = (await _context.ModuleProgress
                .Where(x => x.UserId == userId && moduleIds.Contains(x.ModuleId))
                .ToListAsync(cancellationToken))
                .ToDictionary(x => x.ModuleId);

            var states = new List<LearnerModuleStateInfo>();

            foreach (var module in assignedModules)
            {
                var moduleLessons = lessons
                    .Where(x => x.ModuleId == module.ModuleId)
                    .ToList();

                var totalLessons = moduleLessons.Count;

                var completedLessons = moduleLessons
                    .Count(x => completedLessonIdSet.Contains(x.LessonId));

                var allLessonsCompleted =
                    totalLessons > 0 &&
                    completedLessons == totalLessons;

                var quizPassed = passedModuleIdSet.Contains(module.ModuleId);

                // Module completion requires all lessons and a passed quiz.
                var shouldBeCompleted = allLessonsCompleted && quizPassed;

                progressByModuleId.TryGetValue(
                    module.ModuleId,
                    out var moduleProgress);

                // Self-heal the stored flag if it drifted.
                if (moduleProgress is not null &&
                    moduleProgress.IsCompleted != shouldBeCompleted)
                {
                    moduleProgress.IsCompleted = shouldBeCompleted;
                    moduleProgress.CompletedAt = shouldBeCompleted
                        ? DateTime.UtcNow
                        : null;
                }

                states.Add(new LearnerModuleStateInfo
                {
                    ModuleId = module.ModuleId,
                    ModuleName = module.ModuleName,
                    Description = module.Description,
                    AssignedAt = module.AssignedAt,
                    HasStarted = moduleProgress is not null,
                    StartedAt = moduleProgress?.StartedAt,
                    LastAccessedAt = moduleProgress?.LastAccessedAt,
                    IsCompleted = shouldBeCompleted,
                    CompletedAt = moduleProgress?.CompletedAt,
                    TotalLessons = totalLessons,
                    CompletedLessons = completedLessons,
                    QuizPassed = quizPassed
                });
            }

            if (_context.ChangeTracker.HasChanges())
            {
                await _context.SaveChangesAsync(cancellationToken);
            }

            return states;
        }

        public async Task<int> GetDistinctQuizModuleCountAsync(
            List<int> moduleIds, CancellationToken cancellationToken)
        {
            return await _context.QuizQuestions
                .Where(q => moduleIds.Contains(q.ModuleId))
                .Select(q => q.ModuleId)
                .Distinct()
                .CountAsync(cancellationToken);
        }

        public async Task<int> GetAttemptedQuizModuleCountAsync(
            int userId, List<int> moduleIds, CancellationToken cancellationToken)
        {
            return await _context.QuizAttempts
                .Where(a => a.UserId == userId && moduleIds.Contains(a.ModuleId))
                .Select(a => a.ModuleId)
                .Distinct()
                .CountAsync(cancellationToken);
        }

        public async Task<List<int>> GetCertifiedModuleIdsAsync(
            int userId, CancellationToken cancellationToken)
        {
            return await _context.Certificates
                .Where(c => c.UserId == userId)
                .Select(c => c.ModuleId)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> GetPassedScenarioCountAsync(
            int userId, CancellationToken cancellationToken)
        {
            return await _context.ScenarioAttempts
                .Where(a => a.UserId == userId && a.Passed)
                .CountAsync(cancellationToken);
        }

        public async Task<bool> IsModuleAssignedToLearnerAsync(
            int userId, int moduleId, CancellationToken cancellationToken)
        {
            return await (
                from roleModule in _context.RoleModules
                join user in _context.Users
                    on roleModule.RoleId equals user.RoleId
                join module in _context.Modules
                    on roleModule.ModuleId equals module.ModuleId
                where user.UserId == userId
                      && roleModule.ModuleId == moduleId
                      && module.IsActive
                select roleModule
            ).AnyAsync(cancellationToken);
        }

        public async Task<StartModuleResultInfo> StartModuleAsync(
            int userId, int moduleId, CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;

            var progress = await _context.ModuleProgress
                .SingleOrDefaultAsync(x =>
                    x.UserId == userId &&
                    x.ModuleId == moduleId,
                    cancellationToken);

            if (progress is null)
            {
                progress = new ModuleProgress
                {
                    UserId = userId,
                    ModuleId = moduleId,
                    StartedAt = now,
                    LastAccessedAt = now,
                    IsCompleted = false
                };

                _context.ModuleProgress.Add(progress);
            }
            else
            {
                progress.LastAccessedAt = now;
            }

            await _context.SaveChangesAsync(cancellationToken);

            return new StartModuleResultInfo
            {
                ModuleId = progress.ModuleId,
                StartedAt = progress.StartedAt,
                LastAccessedAt = progress.LastAccessedAt,
                IsCompleted = progress.IsCompleted
            };
        }
    }
}
