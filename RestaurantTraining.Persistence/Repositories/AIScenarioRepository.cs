using Microsoft.EntityFrameworkCore;
using RestaurantTraining.Application.Common.Interfaces;
using RestaurantTraining.Domain.Entities;

namespace RestaurantTraining.Persistence.Repositories
{
    public class AIScenarioRepository : IAIScenarioRepository
    {
        private readonly AppDbContext _context;

        public AIScenarioRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ScenarioGenContext?> GetGenerationContextAsync(
            int moduleId, string roleName, CancellationToken cancellationToken)
        {
            var module = await _context.Modules
                .Where(m => m.ModuleId == moduleId && m.IsActive)
                .Select(m => m.ModuleName)
                .FirstOrDefaultAsync(cancellationToken);

            if (module is null)
                return null;

            var role = await _context.Roles
                .Where(r => r.RoleName == roleName)
                .Select(r => new { r.RoleId, r.RoleName })
                .FirstOrDefaultAsync(cancellationToken);

            if (role is null)
                return null;

            return new ScenarioGenContext
            {
                RoleId = role.RoleId,
                RoleName = role.RoleName,
                ModuleTitle = module
            };
        }

        public async Task<AIScenario?> GetActiveScenarioAsync(
            int moduleId, int roleId, CancellationToken cancellationToken)
        {
            return await _context.AIScenarios
                .FirstOrDefaultAsync(
                    s => s.ModuleId == moduleId && s.RoleId == roleId && s.IsActive,
                    cancellationToken);
        }

        public async Task<int> AddScenarioAsync(
            AIScenario scenario, CancellationToken cancellationToken)
        {
            _context.AIScenarios.Add(scenario);
            await _context.SaveChangesAsync(cancellationToken);
            return scenario.ScenarioId;
        }

        public async Task<AIScenario?> GetScenarioByIdAsync(
            int scenarioId, CancellationToken cancellationToken)
        {
            return await _context.AIScenarios
                .FirstOrDefaultAsync(s => s.ScenarioId == scenarioId, cancellationToken);
        }

        public async Task<bool> HasPassedScenarioAsync(
            int userId, int scenarioId, CancellationToken cancellationToken)
        {
            return await _context.ScenarioAttempts
                .AnyAsync(a => a.UserId == userId && a.ScenarioId == scenarioId && a.Passed,
                    cancellationToken);
        }

        public async Task RecordAttemptAsync(
            int userId, int scenarioId, bool passed, string feedback,
            CancellationToken cancellationToken)
        {
            _context.ScenarioAttempts.Add(new ScenarioAttempt
            {
                UserId = userId,
                ScenarioId = scenarioId,
                Score = passed ? 100 : 0,
                Passed = passed,
                Feedback = feedback,
                AttemptedAt = DateTime.UtcNow
            });

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
