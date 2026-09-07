using MediatR;
using RestaurantTraining.Application.Common.Interfaces;

namespace RestaurantTraining.Application.Features.Lessons.Commands.UpdateLesson
{
    public class UpdateLessonCommandHandler
        : IRequestHandler<UpdateLessonCommand, UpdateLessonResponse>
    {
        private readonly ILessonRepository _lessonRepository;

        public UpdateLessonCommandHandler(
            ILessonRepository lessonRepository)
        {
            _lessonRepository = lessonRepository;
        }

        public async Task<UpdateLessonResponse> Handle(
            UpdateLessonCommand request,
            CancellationToken cancellationToken)
        {
            var lesson = await _lessonRepository.GetLessonByIdAsync(
                request.LessonId,
                cancellationToken);

            if (lesson == null)
            {
                return new UpdateLessonResponse
                {
                    Success = false,
                    Message = "Lesson not found."
                };
            }

            if (string.IsNullOrWhiteSpace(request.LessonTitle))
            {
                return new UpdateLessonResponse
                {
                    Success = false,
                    Message = "Lesson title is required."
                };
            }

            lesson.ModuleId = request.ModuleId;
            lesson.LessonTitle = request.LessonTitle;
            lesson.Description = request.Description;
            lesson.SortOrder = request.SortOrder;
            lesson.IsActive = request.IsActive;

            await _lessonRepository.UpdateLessonAsync(
                lesson,
                cancellationToken);

            return new UpdateLessonResponse
            {
                Success = true,
                Message = "Lesson updated successfully.",
                LessonId = lesson.LessonId
            };
        }
    }
}