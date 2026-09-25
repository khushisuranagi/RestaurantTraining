using MediatR;
using RestaurantTraining.Application.Common.Interfaces;


namespace RestaurantTraining.Application.Features.Modules.Commands.DeleteModule
{
    public class DeleteModuleCommandHandler
        : IRequestHandler<DeleteModuleCommand, DeleteModuleResponse>
    {
        private readonly IModuleRepository _moduleRepository;

        public DeleteModuleCommandHandler(IModuleRepository moduleRepository)
        {
            _moduleRepository = moduleRepository;
        }

        public async Task<DeleteModuleResponse> Handle(
            DeleteModuleCommand request,
            CancellationToken cancellationToken)
        {
            var module = await _moduleRepository.GetModuleByIdAsync(
                request.ModuleId,
                cancellationToken);

            if (module == null)
            {
                return new DeleteModuleResponse
                {
                    Success = false,
                    Message = "Module not found."
                };
            }

            var lessonCount = await _moduleRepository.GetLessonCountForModuleAsync(
                request.ModuleId, cancellationToken);

            if (lessonCount > 0 && !request.ConfirmCascade)
            {
                return new DeleteModuleResponse
                {
                    Success = false,
                    Message = $"This module has {lessonCount} lesson(s). Deleting it will also remove " +
                              "all its lessons, resources, quizzes, scenarios and learner progress.",//$"This module still has {lessonCount} lesson(s). Delete all lessons in this module before deleting the module itself.",
                    RequiresConfirmation = true,
                    LessonCount = lessonCount
                
                };
            }

            await _moduleRepository.DeleteModuleWithChildrenAsync(request.ModuleId, cancellationToken);

            return new DeleteModuleResponse
            {
                Success = true,
                Message = "Module deleted successfully."
            };
        }
    }
}