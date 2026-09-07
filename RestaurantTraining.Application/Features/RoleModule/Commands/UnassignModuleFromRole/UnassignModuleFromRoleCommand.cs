using MediatR;
using RestaurantTraining.Application.Common.Responses;

namespace RestaurantTraining.Application.Features.RoleModules.Commands.UnassignModuleFromRole
{
    public class UnassignModuleFromRoleCommand : IRequest<BaseResponse>
    {
        public int RoleId { get; set; }
        public int ModuleId { get; set; }
    }
}