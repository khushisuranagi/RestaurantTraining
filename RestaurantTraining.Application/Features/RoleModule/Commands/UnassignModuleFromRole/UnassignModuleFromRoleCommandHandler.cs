using MediatR;
using RestaurantTraining.Application.Common.Interfaces;
using RestaurantTraining.Application.Common.Responses;

namespace RestaurantTraining.Application.Features.RoleModules.Commands.UnassignModuleFromRole
{
    public class UnassignModuleFromRoleCommandHandler
        : IRequestHandler<UnassignModuleFromRoleCommand, BaseResponse>
    {
        private readonly IRoleModuleRepository _roleModuleRepository;

        public UnassignModuleFromRoleCommandHandler(IRoleModuleRepository roleModuleRepository)
        {
            _roleModuleRepository = roleModuleRepository;
        }

        public async Task<BaseResponse> Handle(
            UnassignModuleFromRoleCommand request,
            CancellationToken cancellationToken)
        {
            var removed = await _roleModuleRepository.RemoveAsync(
                request.RoleId, request.ModuleId, cancellationToken);

            if (!removed)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "That assignment was not found."
                };
            }

            return new BaseResponse
            {
                Success = true,
                Message = "Module unassigned from role successfully."
            };
        }
    }
}