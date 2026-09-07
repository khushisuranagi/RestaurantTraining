using Microsoft.EntityFrameworkCore;
using RestaurantTraining.Application.Common.Interfaces;
using RestaurantTraining.Domain.Entities;

namespace RestaurantTraining.Persistence.Repositories
{
    public class LearnerQuizRepository : ILearnerQuizRepository
    {
        private readonly AppDbContext _context;

        public LearnerQuizRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> IsAssignedToLearnerAsync(
            string roleName, int moduleId, CancellationToken cancellationToken)
        {
            return await (
                from rm in _context.RoleModules
                join r in _context.Roles on rm.RoleId equals r.RoleId
                where r.RoleName == roleName && rm.ModuleId == moduleId
                select rm
            ).AnyAsync(cancellationToken);
        }

        public async Task<List<QuizQuestionInfo>> GetQuizQuestionsAsync(
            int moduleId, CancellationToken cancellationToken)
        {
            return await _context.QuizQuestions
                .Where(q => q.ModuleId == moduleId)
                .OrderBy(q => q.QuestionId)
                .Select(q => new QuizQuestionInfo
                {
                    QuestionId = q.QuestionId,
                    QuestionText = q.QuestionText,
                    QuestionType = (int)q.QuestionType,
                    Marks = q.Marks,
                    Options = _context.QuizOptions
                        .Where(o => o.QuestionId == q.QuestionId)
                        .Select(o => new QuizOptionInfo
                        {
                            OptionId = o.OptionId,
                            OptionText = o.OptionText   // no IsCorrect!
                        })
                        .ToList()
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<List<GradingQuestionInfo>> GetGradingQuestionsAsync(
            int moduleId, CancellationToken cancellationToken)
        {
            return await _context.QuizQuestions
                .Where(q => q.ModuleId == moduleId)
                .Select(q => new GradingQuestionInfo
                {
                    QuestionId = q.QuestionId,
                    QuestionType = q.QuestionType,
                    Marks = q.Marks,
                    Options = _context.QuizOptions
                        .Where(o => o.QuestionId == q.QuestionId)
                        .Select(o => new GradingOptionInfo
                        {
                            OptionId = o.OptionId,
                            OptionText = o.OptionText,
                            IsCorrect = o.IsCorrect
                        })
                        .ToList()
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<int?> GetPassingScoreAsync(
            int moduleId, CancellationToken cancellationToken)
        {
            return await _context.ModuleCertificateSettings
                .Where(s => s.ModuleId == moduleId)
                .Select(s => (int?)s.MinimumPassingScore)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<bool> HasCertificateAsync(
            int userId, int moduleId, CancellationToken cancellationToken)
        {
            return await _context.Certificates
                .AnyAsync(c => c.UserId == userId && c.ModuleId == moduleId, cancellationToken);
        }

        public async Task RecordAttemptAsync(
            int userId, int moduleId, decimal score, decimal totalMarks,
            bool passed, string? certificateNumber, CancellationToken cancellationToken)
        {
            _context.QuizAttempts.Add(new QuizAttempt
            {
                UserId = userId,
                ModuleId = moduleId,
                Score = score,
                TotalMarks = totalMarks,
                Passed = passed,
                AttemptedAt = DateTime.UtcNow
            });

            if (certificateNumber is not null)
            {
                _context.Certificates.Add(new Certificate
                {
                    UserId = userId,
                    ModuleId = moduleId,
                    CertificateNumber = certificateNumber,
                    IssuedDate = DateTime.UtcNow
                });
            }

            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<string> GetModuleNameAsync(
            int moduleId, CancellationToken cancellationToken)
        {
            return await _context.Modules
                .Where(m => m.ModuleId == moduleId)
                .Select(m => m.ModuleName)
                .FirstOrDefaultAsync(cancellationToken) ?? string.Empty;
        }
    }
}
