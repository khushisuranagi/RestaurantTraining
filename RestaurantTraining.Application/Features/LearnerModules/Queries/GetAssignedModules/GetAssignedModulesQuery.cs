using MediatR;

namespace RestaurantTraining.Application.Features.LearnerModules.Queries.GetAssignedModules
{
    public class GetAssignedModulesQuery : IRequest<List<AssignedModuleDto>>
    {
        public string RoleName { get; set; } = string.Empty;
    }
}
