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

            var lessonCount = await _moduleRepository.GetLessonCountForModuleAsync(
                request.ModuleId, cancellationToken);

            if (lessonCount > 0)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = $"This module still has {lessonCount} lesson(s). Delete all lessons in this module before deleting the module itself."
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