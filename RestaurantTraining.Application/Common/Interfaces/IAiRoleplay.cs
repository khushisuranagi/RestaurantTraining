namespace RestaurantTraining.Application.Common.Interfaces
{
    // The PORT for the AI practice scenario. Infrastructure (Gemini) provides
    // the adapter; the Application layer knows nothing about how it works.
    public interface IAiRoleplay
    {
        // Create a role-appropriate customer scenario for a module + role.
        Task<GeneratedScenario?> GenerateScenarioAsync(
            string moduleTitle, string roleName, CancellationToken cancellationToken);

        // Continue the chat as the customer, and judge if satisfied.
        Task<RoleplayTurn?> ContinueAsync(
            RoleplayContext context, CancellationToken cancellationToken);
    }

    public class GeneratedScenario
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;   // shown to the learner
        public string ScenarioPrompt { get; set; } = string.Empty; // hidden system instruction
        public string Category { get; set; } = string.Empty;
    }

    public class RoleplayContext
    {
        public string ScenarioPrompt { get; set; } = string.Empty;
        public List<RoleplayMessage> Messages { get; set; } = [];
    }

    public class RoleplayMessage
    {
        public bool FromLearner { get; set; }   // true = learner, false = AI customer
        public string Text { get; set; } = string.Empty;
    }

    public class RoleplayTurn
    {
        public string Reply { get; set; } = string.Empty;   // customer's next message
        public bool Satisfied { get; set; }                  // issue resolved well?
        public string Feedback { get; set; } = string.Empty; // coaching note
    }
}
