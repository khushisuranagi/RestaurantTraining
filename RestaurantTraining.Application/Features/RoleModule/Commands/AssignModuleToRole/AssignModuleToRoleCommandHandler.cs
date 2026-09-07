using MediatR;
using RestaurantTraining.Application.Common.Interfaces;
using RestaurantTraining.Domain.Entities;

namespace RestaurantTraining.Application.Features.RoleModules.Commands.AssignModuleToRole
{
    public class AssignModuleToRoleCommandHandler
        : IRequestHandler<AssignModuleToRoleCommand, AssignModuleToRoleResponse>
    {
        private readonly IRoleModuleRepository _roleModuleRepository;

        public AssignModuleToRoleCommandHandler(IRoleModuleRepository roleModuleRepository)
        {
            _roleModuleRepository = roleModuleRepository;
        }

        public async Task<AssignModuleToRoleResponse> Handle(
            AssignModuleToRoleCommand request,
            CancellationToken cancellationToken)
        {
            // Don't assign the same module to the same role twice.
            var alreadyAssigned = await _roleModuleRepository.ExistsAsync(
                request.RoleId, request.ModuleId, cancellationToken);

            if (alreadyAssigned)
            {
                return new AssignModuleToRoleResponse
                {
                    Success = false,
                    Message = "This module is already assigned to that role."
                };
            }

            var roleModule = new RoleModule
            {
                RoleId = request.RoleId,
                ModuleId = request.ModuleId,
                AssignedAt = DateTime.UtcNow
            };

            await _roleModuleRepository.AssignAsync(roleModule, cancellationToken);

            return new AssignModuleToRoleResponse
            {
                Success = true,
                Message = "Module assigned to role successfully."
            };
        }
    }
}