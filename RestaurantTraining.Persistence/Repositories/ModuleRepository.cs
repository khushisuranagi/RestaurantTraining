using Microsoft.EntityFrameworkCore;
using RestaurantTraining.Application.Common.Interfaces;
using RestaurantTraining.Domain.Entities;

namespace RestaurantTraining.Persistence.Repositories
{
    public class ModuleRepository : IModuleRepository
    {
        private readonly AppDbContext _context;

        public ModuleRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddModuleAsync(
            Module module,
            CancellationToken cancellationToken)
        {
            await _context.Modules.AddAsync(module, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            // After saving, EF fills in the new ModuleId for us.
            return module.ModuleId;
        }

        public async Task<List<Module>> GetAllModulesAsync(
            CancellationToken cancellationToken)
        {
            return await _context.Modules
                .ToListAsync(cancellationToken);
        }

        public async Task<Module?> GetModuleByIdAsync(
            int moduleId,
            CancellationToken cancellationToken)
        {
            return await _context.Modules
                .FirstOrDefaultAsync(
                    x => x.ModuleId == moduleId,
                    cancellationToken);
        }

        public async Task UpdateModuleAsync(
            Module module,
            CancellationToken cancellationToken)
        {
            _context.Modules.Update(module);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteModuleAsync(
            Module module,
            CancellationToken cancellationToken)
        {
            _context.Modules.Remove(module);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> GetLessonCountForModuleAsync(
    int moduleId,
    CancellationToken cancellationToken)
        {
            return await _context.Lessons
                .CountAsync(l => l.ModuleId == moduleId, cancellationToken);
        }


        public async Task DeleteModuleWithChildrenAsync(
    int moduleId, CancellationToken cancellationToken)
        {
            await using var transaction =
                await _context.Database.BeginTransactionAsync(cancellationToken);

            // Ids needed for the grand-child deletes.
            var lessonIds = await _context.Lessons
                .Where(l => l.ModuleId == moduleId).Select(l => l.LessonId).ToListAsync(cancellationToken);

            var resourceIds = await _context.LessonResources
                .Where(r => lessonIds.Contains(r.LessonId)).Select(r => r.ResourceId).ToListAsync(cancellationToken);

            var questionIds = await _context.QuizQuestions
                .Where(q => q.ModuleId == moduleId).Select(q => q.QuestionId).ToListAsync(cancellationToken);

            var scenarioIds = await _context.AIScenarios
                .Where(s => s.ModuleId == moduleId).Select(s => s.ScenarioId).ToListAsync(cancellationToken);

            // ---- delete children first, then parents ----

            // AI scenarios
            await _context.ScenarioAttempts.Where(a => scenarioIds.Contains(a.ScenarioId))
                .ExecuteDeleteAsync(cancellationToken);
            await _context.AIScenarios.Where(s => s.ModuleId == moduleId)
                .ExecuteDeleteAsync(cancellationToken);

            // Quiz
            await _context.QuizOptions.Where(o => questionIds.Contains(o.QuestionId))
                .ExecuteDeleteAsync(cancellationToken);
            await _context.QuizQuestions.Where(q => q.ModuleId == moduleId)
                .ExecuteDeleteAsync(cancellationToken);
            await _context.QuizAttempts.Where(a => a.ModuleId == moduleId)
                .ExecuteDeleteAsync(cancellationToken);

            // Module-level records
            await _context.Certificates.Where(c => c.ModuleId == moduleId)
                .ExecuteDeleteAsync(cancellationToken);
            await _context.ModuleProgress.Where(p => p.ModuleId == moduleId)
                .ExecuteDeleteAsync(cancellationToken);
            await _context.ModuleCertificateSettings.Where(s => s.ModuleId == moduleId)
                .ExecuteDeleteAsync(cancellationToken);

            // Per-resource AI content (no FK, but remove orphans)
            await _context.ResourceQuestionAttempts.Where(a => resourceIds.Contains(a.ResourceId))
                .ExecuteDeleteAsync(cancellationToken);
            await _context.ResourceQuestionOptions
                .Where(o => _context.ResourceQuestions
                    .Where(q => resourceIds.Contains(q.ResourceId))
                    .Select(q => q.ResourceQuestionId)
                    .Contains(o.ResourceQuestionId))
                .ExecuteDeleteAsync(cancellationToken);
            await _context.ResourceQuestions.Where(q => resourceIds.Contains(q.ResourceId))
                .ExecuteDeleteAsync(cancellationToken);
            await _context.ResourceSummaries.Where(s => resourceIds.Contains(s.ResourceId))
                .ExecuteDeleteAsync(cancellationToken);

            // Lessons + their progress/resources
            await _context.LessonProgress.Where(p => lessonIds.Contains(p.LessonId))
                .ExecuteDeleteAsync(cancellationToken);
            await _context.LessonResources.Where(r => lessonIds.Contains(r.LessonId))
                .ExecuteDeleteAsync(cancellationToken);
            await _context.Lessons.Where(l => l.ModuleId == moduleId)
                .ExecuteDeleteAsync(cancellationToken);

            // Finally the module (RoleModules cascades automatically at the DB level)
            await _context.Modules.Where(m => m.ModuleId == moduleId)
                .ExecuteDeleteAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);
        }

    }
}
