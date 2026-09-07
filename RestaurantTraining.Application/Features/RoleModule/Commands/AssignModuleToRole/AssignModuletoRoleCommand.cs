using MediatR;

namespace RestaurantTraining.Application.Features.RoleModules.Commands.AssignModuleToRole
{
    public class AssignModuleToRoleCommand : IRequest<AssignModuleToRoleResponse>
    {
        public int RoleId { get; set; }
        public int ModuleId { get; set; }
    }
}