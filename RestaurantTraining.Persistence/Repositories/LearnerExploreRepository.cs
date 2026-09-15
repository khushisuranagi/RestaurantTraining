using Microsoft.EntityFrameworkCore;
using RestaurantTraining.Application.Common.Interfaces;

namespace RestaurantTraining.Persistence.Repositories
{
    public class LearnerExploreRepository : ILearnerExploreRepository
    {
        private readonly AppDbContext _context;

        public LearnerExploreRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ExploreModuleInfo>> GetUnassignedModulesAsync(
            string roleName, CancellationToken cancellationToken)
        {
            // Module ids assigned to this learner's role.
            var assignedModuleIds =
                from rm in _context.RoleModules
                join r in _context.Roles on rm.RoleId equals r.RoleId
                where r.RoleName == roleName
                select rm.ModuleId;

            // Active modules that are NOT in that set.
            return await _context.Modules
                .Where(m => m.IsActive && !assignedModuleIds.Contains(m.ModuleId))
                .OrderBy(m => m.ModuleName)
                .Select(m => new ExploreModuleInfo
                {
                    ModuleId = m.ModuleId,
                    ModuleName = m.ModuleName,
                    Description = m.Description,
                    CoverImageData = m.CoverImageData,
                    CoverImageContentType = m.CoverImageContentType
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<ModuleContentInfo?> GetModuleContentAsync(
            int moduleId, CancellationToken cancellationToken)
        {
            // Any active module — no role gate. Read only, so IsCompleted is false.
            return await _context.Modules
                .Where(m => m.ModuleId == moduleId && m.IsActive)
                .Select(m => new ModuleContentInfo
                {
                    ModuleId = m.ModuleId,
                    ModuleName = m.ModuleName,
                    Description = m.Description,
                    Lessons = _context.Lessons
                        .Where(lesson => lesson.ModuleId == m.ModuleId && lesson.IsActive)
                        .OrderBy(lesson => lesson.SortOrder)
                        .Select(lesson => new ModuleLessonInfo
                        {
                            LessonId = lesson.LessonId,
                            LessonTitle = lesson.LessonTitle,
                            Description = lesson.Description,
                            SortOrder = lesson.SortOrder,
                            IsCompleted = false,
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
    }
}
