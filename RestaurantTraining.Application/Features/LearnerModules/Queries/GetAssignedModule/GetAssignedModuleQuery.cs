using MediatR;

namespace RestaurantTraining.Application.Features.LearnerModules.Queries.GetAssignedModule
{
    // Returns null when the module is not assigned to the learner's role
    // (the controller turns null into a 404 Not Found).
    public class GetAssignedModuleQuery : IRequest<ModuleContentDto?>
    {
        public string RoleName { get; set; } = string.Empty;
        public int UserId { get; set; }
        public int ModuleId { get; set; }
    }
}
