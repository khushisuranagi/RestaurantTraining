using MediatR;
using RestaurantTraining.Application.Common.Interfaces;

namespace RestaurantTraining.Application.Features.LearnerModules.Commands.CompleteLesson
{
    public class CompleteLessonCommandHandler
        : IRequestHandler<CompleteLessonCommand, CompleteLessonResultDto>
    {
        private readonly ILearnerModuleRepository _repository;

        public CompleteLessonCommandHandler(
            ILearnerModuleRepository repository)
        {
            _repository = repository;
        }

        public async Task<CompleteLessonResultDto> Handle(
            CompleteLessonCommand request,
            CancellationToken cancellationToken)
        {
            var done = await _repository.CompleteLessonAsync(
                request.UserId, request.LessonId, cancellationToken);

            if (!done)
            {
                return new CompleteLessonResultDto { LessonNotFound = true };
            }

            return new CompleteLessonResultDto
            {
                LessonNotFound = false,
                LessonId = request.LessonId,
                IsCompleted = true
            };
        }
    }
}
