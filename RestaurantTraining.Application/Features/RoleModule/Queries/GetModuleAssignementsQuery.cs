using MediatR;

namespace RestaurantTraining.Application.Features.RoleModules.Queries.GetModuleAssignments
{
    public class GetModuleAssignmentsQuery : IRequest<List<ModuleAssignmentDto>>
    {
        public int ModuleId { get; set; }
    }
}