using MediatR;
using RestaurantTraining.Application.Common.Interfaces;
using RestaurantTraining.Application.Common.Responses;

namespace RestaurantTraining.Application.Features.Modules.Commands.UpdateModule
{
    public class UpdateModuleCommandHandler
        : IRequestHandler<UpdateModuleCommand, BaseResponse>
    {
        private readonly IModuleRepository _moduleRepository;

        public UpdateModuleCommandHandler(IModuleRepository moduleRepository)
        {
            _moduleRepository = moduleRepository;
        }

        public async Task<BaseResponse> Handle(
            UpdateModuleCommand request,
            CancellationToken cancellationToken)
        {
            // First find the module we want to update.
            var module = await _moduleRepository.GetModuleByIdAsync(
                request.ModuleId,
                cancellationToken);

            if (module == null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Module not found."
                };
            }

            // Change its values, then save.
            module.ModuleName = request.ModuleName;
            module.Description = request.Description;
            module.IsActive = request.IsActive;

            await _moduleRepository.UpdateModuleAsync(module, cancellationToken);

            return new BaseResponse
            {
                Success = true,
                Message = "Module updated successfully."
            };
        }
    }
}
