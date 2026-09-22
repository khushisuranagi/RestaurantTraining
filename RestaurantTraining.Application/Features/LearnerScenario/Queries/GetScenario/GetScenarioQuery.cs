using MediatR;

namespace RestaurantTraining.Application.Features.LearnerScenario.Queries.GetScenario
{
    // Gets (or generates + stores) the practice scenario for this learner's
    // role on the given module. Same scenario is reused for everyone of that role.
    public class GetScenarioQuery : IRequest<ScenarioResult>
    {
        public int ModuleId { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public int UserId { get; set; }
    }

    public class ScenarioResult
    {
        public bool NotAvailable { get; set; }   // → controller 404
        public ScenarioDto? Scenario { get; set; }
    }

    public class ScenarioDto
    {
        public int ScenarioId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool AlreadyPassed { get; set; }
    }
}
