using Microsoft.EntityFrameworkCore;
using RestaurantTraining.Application.Common.Interfaces;
using RestaurantTraining.Domain.Entities;

namespace RestaurantTraining.Persistence.Repositories
{
    public class CertificateIssuanceRepository : ICertificateIssuanceRepository
    {
        private readonly AppDbContext _context;

        public CertificateIssuanceRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> HasPassedModuleQuizAsync(
            int userId, int moduleId, CancellationToken cancellationToken)
        {
            return await _context.QuizAttempts
                .AnyAsync(a => a.UserId == userId && a.ModuleId == moduleId && a.Passed,
                    cancellationToken);
        }

        public async Task<bool> HasCertificateAsync(
            int userId, int moduleId, CancellationToken cancellationToken)
        {
            return await _context.Certificates
                .AnyAsync(c => c.UserId == userId && c.ModuleId == moduleId, cancellationToken);
        }

        public async Task IssueCertificateAsync(
            int userId, int moduleId, string certificateNumber, CancellationToken cancellationToken)
        {
            _context.Certificates.Add(new Certificate
            {
                UserId = userId,
                ModuleId = moduleId,
                CertificateNumber = certificateNumber,
                IssuedDate = DateTime.UtcNow
            });

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
