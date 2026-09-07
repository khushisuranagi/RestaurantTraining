using Microsoft.EntityFrameworkCore;
using RestaurantTraining.Application.Common.Interfaces;
using RestaurantTraining.Domain.Entities;

namespace RestaurantTraining.Persistence.Repositories
{
    public class LearnerModuleRepository : ILearnerModuleRepository
    {
        private readonly AppDbContext _context;

        public LearnerModuleRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<AssignedModuleInfo>> GetAssignedModulesAsync(
            string roleName, CancellationToken cancellationToken)
        {
            return await (
                from roleModule in _context.RoleModules
                join role in _context.Roles on roleModule.RoleId equals role.RoleId
                join module in _context.Modules on roleModule.ModuleId equals module.ModuleId
                where role.RoleName == roleName && module.IsActive
                orderby roleModule.AssignedAt descending
                select new AssignedModuleInfo
                {
                    ModuleId = module.ModuleId,
                    ModuleName = module.ModuleName,
                    Description = module.Description,
                    CreatedAt = module.CreatedAt
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<ModuleContentInfo?> GetAssignedModuleAsync(
            string roleName, int userId, int moduleId, CancellationToken cancellationToken)
        {
            return await (
                from roleModule in _context.RoleModules
                join role in _context.Roles on roleModule.RoleId equals role.RoleId
                join trainingModule in _context.Modules on roleModule.ModuleId equals trainingModule.ModuleId
                where role.RoleName == roleName
                      && trainingModule.ModuleId == moduleId
                      && trainingModule.IsActive
                select new ModuleContentInfo
                {
                    ModuleId = trainingModule.ModuleId,
                    ModuleName = trainingModule.ModuleName,
                    Description = trainingModule.Description,
                    Lessons = _context.Lessons
                        .Where(lesson => lesson.ModuleId == trainingModule.ModuleId && lesson.IsActive)
                        .OrderBy(lesson => lesson.SortOrder)
                        .Select(lesson => new ModuleLessonInfo
                        {
                            LessonId = lesson.LessonId,
                            LessonTitle = lesson.LessonTitle,
                            Description = lesson.Description,
                            SortOrder = lesson.SortOrder,
                            IsCompleted = _context.LessonProgress
                                .Any(p => p.UserId == userId
                                          && p.LessonId == lesson.LessonId
                                          && p.IsCompleted),
                            Resources = _context.LessonResources
                                .Where(resource => resource.LessonId == lesson.LessonId && resource.IsActive)
                                .OrderBy(resource => resource.SortOrder)
                                .Select(resource => new ModuleResourceInfo
                                {
                                    ResourceId = resource.ResourceId,
                                    ResourceType = (int)resource.ResourceType,
                                    ResourceUrl = resource.ResourceUrl,
                                    FileData = resource.FileData,
                                    FileName = resource.FileName,
                                    ContentType = resource.ContentType,
                                    ContentText = resource.ContentText,
                                    SortOrder = resource.SortOrder
                                })
                                .ToList()
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<bool> CompleteLessonAsync(
            int userId, int lessonId, CancellationToken cancellationToken)
        {
            var lesson = await _context.Lessons
                .FirstOrDefaultAsync(l => l.LessonId == lessonId && l.IsActive, cancellationToken);

            if (lesson is null)
                return false;

            var now = DateTime.UtcNow;

            // 1) Mark the lesson complete.
            var progress = await _context.LessonProgress
                .FirstOrDefaultAsync(
                    p => p.UserId == userId && p.LessonId == lessonId,
                    cancellationToken);

            if (progress is null)
            {
                _context.LessonProgress.Add(new LessonProgress
                {
                    UserId = userId,
                    LessonId = lessonId,
                    IsCompleted = true,
                    CompletedAt = now
                });
            }
            else if (!progress.IsCompleted)
            {
                progress.IsCompleted = true;
                progress.CompletedAt = now;
            }

            // 2) Mark the module as started (so it counts as "pending", not "assigned").
            var moduleProgress = await _context.ModuleProgress
                .FirstOrDefaultAsync(
                    mp => mp.UserId == userId && mp.ModuleId == lesson.ModuleId,
                    cancellationToken);

            if (moduleProgress is null)
            {
                _context.ModuleProgress.Add(new ModuleProgress
                {
                    UserId = userId,
                    ModuleId = lesson.ModuleId,
                    StartedAt = now,
                    LastAccessedAt = now,
                    IsCompleted = false
                });
            }
            else
            {
                moduleProgress.LastAccessedAt = now;
            }

            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
