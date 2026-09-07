using MediatR;
using RestaurantTraining.Application.Common.Interfaces;
using RestaurantTraining.Application.Common.Responses;

namespace RestaurantTraining.Application.Features.Modules.Commands.DeleteModule
{
    public class DeleteModuleCommandHandler
        : IRequestHandler<DeleteModuleCommand, BaseResponse>
    {
        private readonly IModuleRepository _moduleRepository;

        public DeleteModuleCommandHandler(IModuleRepository moduleRepository)
        {
            _moduleRepository = moduleRepository;
        }

        public async Task<BaseResponse> Handle(
            DeleteModuleCommand request,
            CancellationToken cancellationToken)
        {
            // Find the module first so we know it exists.
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

            await _moduleRepository.DeleteModuleAsync(module, cancellationToken);

            return new BaseResponse
            {
                Success = true,
                Message = "Module deleted successfully."
            };
        }
    }
}
