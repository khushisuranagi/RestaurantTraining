using RestaurantTraining.Domain.Entities;

namespace RestaurantTraining.Application.Common.Interfaces
{
    public interface IAIScenarioRepository
    {
        Task<int> AddAIScenarioAsync(
            AIScenario scenario,
            CancellationToken cancellationToken);

        Task<List<AIScenario>> GetAllAIScenariosAsync(
            CancellationToken cancellationToken);

        Task<AIScenario?> GetAIScenarioByIdAsync(
            int scenarioId,
            CancellationToken cancellationToken);

        Task UpdateAIScenarioAsync(
            AIScenario scenario,
            CancellationToken cancellationToken);

        Task DeleteAIScenarioAsync(
            AIScenario scenario,
            CancellationToken cancellationToken);
    }
}