using Microsoft.EntityFrameworkCore;
using RestaurantTraining.Application.Common.Interfaces;

namespace RestaurantTraining.Persistence.Repositories
{
    public class LearnerCertificateRepository : ILearnerCertificateRepository
    {
        private readonly AppDbContext _context;

        public LearnerCertificateRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<string> GetLearnerNameAsync(
            int userId, CancellationToken cancellationToken)
        {
            return await _context.Users
                .Where(u => u.UserId == userId)
                .Select(u => u.FullName)
                .FirstOrDefaultAsync(cancellationToken) ?? string.Empty;
        }

        public async Task<List<LearnerCertificateInfo>> GetCertificatesAsync(
            int userId, CancellationToken cancellationToken)
        {
            return await (
                from c in _context.Certificates
                join m in _context.Modules on c.ModuleId equals m.ModuleId
                where c.UserId == userId
                orderby c.IssuedDate descending
                select new LearnerCertificateInfo
                {
                    CertificateId = c.CertificateId,
                    CertificateNumber = c.CertificateNumber,
                    ModuleId = c.ModuleId,
                    ModuleName = m.ModuleName,
                    IssuedDate = c.IssuedDate
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<List<LearnerAttemptInfo>> GetPassedAttemptsAsync(
            int userId, List<int> moduleIds, CancellationToken cancellationToken)
        {
            return await _context.QuizAttempts
                .Where(a => a.UserId == userId
                    && a.Passed
                    && moduleIds.Contains(a.ModuleId))
                .Select(a => new LearnerAttemptInfo
                {
                    ModuleId = a.ModuleId,
                    Score = a.Score,
                    TotalMarks = a.TotalMarks
                })
                .ToListAsync(cancellationToken);
        }
    }
}
