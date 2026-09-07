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

        public async Task<int> AddAIScenarioAsync(
            AIScenario scenario,
            CancellationToken cancellationToken)
        {
            await _context.AIScenarios.AddAsync(
                scenario,
                cancellationToken);

            await _context.SaveChangesAsync(
                cancellationToken);

            return scenario.ScenarioId;
        }

        public async Task<List<AIScenario>> GetAllAIScenariosAsync(
            CancellationToken cancellationToken)
        {
            return await _context.AIScenarios
                .ToListAsync(cancellationToken);
        }

        public async Task<AIScenario?> GetAIScenarioByIdAsync(
            int scenarioId,
            CancellationToken cancellationToken)
        {
            return await _context.AIScenarios
                .FirstOrDefaultAsync(
                    x => x.ScenarioId == scenarioId,
                    cancellationToken);
        }

        public async Task UpdateAIScenarioAsync(
            AIScenario scenario,
            CancellationToken cancellationToken)
        {
            _context.AIScenarios.Update(scenario);

            await _context.SaveChangesAsync(
                cancellationToken);
        }

        public async Task DeleteAIScenarioAsync(
            AIScenario scenario,
            CancellationToken cancellationToken)
        {
            _context.AIScenarios.Remove(scenario);

            await _context.SaveChangesAsync(
                cancellationToken);
        }
    }
}