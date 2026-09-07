using Microsoft.EntityFrameworkCore;
using RestaurantTraining.Application.Common.Interfaces;

namespace RestaurantTraining.Persistence.Repositories
{
    public class ContentCreatorDashboardRepository : IContentCreatorDashboardRepository
    {
        private readonly AppDbContext _context;

        public ContentCreatorDashboardRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<DashboardModuleInfo>> GetModulesAsync(
            CancellationToken cancellationToken)
        {
            return await _context.Modules
                .Select(m => new DashboardModuleInfo
                {
                    ModuleId = m.ModuleId,
                    ModuleName = m.ModuleName,
                    Description = m.Description,
                    IsActive = m.IsActive,
                    CreatedAt = m.CreatedAt
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<List<DashboardLessonInfo>> GetActiveLessonsAsync(
            CancellationToken cancellationToken)
        {
            return await _context.Lessons
                .Where(l => l.IsActive)
                .Select(l => new DashboardLessonInfo
                {
                    LessonId = l.LessonId,
                    LessonTitle = l.LessonTitle,
                    ModuleId = l.ModuleId
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<List<int>> GetQuizQuestionModuleIdsAsync(
            CancellationToken cancellationToken)
        {
            return await _context.QuizQuestions
                .Select(q => q.ModuleId)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<int>> GetAssignedModuleIdsAsync(
            CancellationToken cancellationToken)
        {
            return await _context.RoleModules
                .Select(rm => rm.ModuleId)
                .Distinct()
                .ToListAsync(cancellationToken);
        }

        public async Task<List<int>> GetAssignedRoleIdsAsync(
            CancellationToken cancellationToken)
        {
            return await _context.RoleModules
                .Select(rm => rm.RoleId)
                .Distinct()
                .ToListAsync(cancellationToken);
        }

        public async Task<List<int>> GetAllUserRoleIdsAsync(
            CancellationToken cancellationToken)
        {
            return await _context.Users
                .Select(u => u.RoleId)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<int>> GetLessonIdsWithResourcesAsync(
            CancellationToken cancellationToken)
        {
            return await _context.LessonResources
                .Where(r => r.IsActive)
                .Select(r => r.LessonId)
                .Distinct()
                .ToListAsync(cancellationToken);
        }

        public async Task<int> GetCertificateCountAsync(
            CancellationToken cancellationToken)
        {
            return await _context.Certificates.CountAsync(cancellationToken);
        }

        public async Task<int> GetDistinctCertificateLearnerCountAsync(
            CancellationToken cancellationToken)
        {
            return await _context.Certificates
                .Select(c => c.UserId)
                .Distinct()
                .CountAsync(cancellationToken);
        }

        public async Task<int> GetActiveLearnerCountAsync(
            CancellationToken cancellationToken)
        {
            return await _context.LessonProgress
                .Where(p => p.IsCompleted)
                .Select(p => p.UserId)
                .Distinct()
                .CountAsync(cancellationToken);
        }

        public async Task<List<DashboardRecentCertificateInfo>> GetRecentCertificatesAsync(
            int take, CancellationToken cancellationToken)
        {
            return await (
                from c in _context.Certificates
                join u in _context.Users on c.UserId equals u.UserId
                join m in _context.Modules on c.ModuleId equals m.ModuleId
                orderby c.IssuedDate descending
                select new DashboardRecentCertificateInfo
                {
                    LearnerName = u.FullName,
                    ModuleName = m.ModuleName,
                    IssuedDate = c.IssuedDate
                })
                .Take(take)
                .ToListAsync(cancellationToken);
        }
    }
}
