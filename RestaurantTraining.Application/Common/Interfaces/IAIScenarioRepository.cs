using RestaurantTraining.Domain.Entities;

namespace RestaurantTraining.Application.Common.Interfaces
{
    // Data access for the AI practice scenarios.
    public interface IAIScenarioRepository
    {
        // The context needed to generate/key a scenario for a learner's role + module.
        // Null if the module or role can't be found.
        Task<ScenarioGenContext?> GetGenerationContextAsync(
            int moduleId, string roleName, CancellationToken cancellationToken);

        // The single active scenario for a (module, role), or null if none yet.
        Task<AIScenario?> GetActiveScenarioAsync(
            int moduleId, int roleId, CancellationToken cancellationToken);

        // Store a newly generated scenario; returns its new id.
        Task<int> AddScenarioAsync(
            AIScenario scenario, CancellationToken cancellationToken);

        Task<AIScenario?> GetScenarioByIdAsync(
            int scenarioId, CancellationToken cancellationToken);

        // Has this learner already passed this scenario?
        Task<bool> HasPassedScenarioAsync(
            int userId, int scenarioId, CancellationToken cancellationToken);

        // Record an attempt result (pass or fail).
        Task RecordAttemptAsync(
            int userId, int scenarioId, bool passed, string feedback,
            CancellationToken cancellationToken);
    }

    public class ScenarioGenContext
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public string ModuleTitle { get; set; } = string.Empty;
    }
}
