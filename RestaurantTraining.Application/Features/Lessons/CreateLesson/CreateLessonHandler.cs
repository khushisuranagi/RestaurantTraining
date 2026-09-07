using MediatR;
using RestaurantTraining.Application.Common.Interfaces;
using RestaurantTraining.Domain.Entities;

namespace RestaurantTraining.Application.Features.Lessons.Commands.CreateLesson
{
    public class CreateLessonCommandHandler
        : IRequestHandler<CreateLessonCommand, CreateLessonResponse>
    {
        private readonly ILessonRepository _lessonRepository;

        public CreateLessonCommandHandler(
            ILessonRepository lessonRepository)
        {
            _lessonRepository = lessonRepository;
        }

        public async Task<CreateLessonResponse> Handle(
            CreateLessonCommand request,
            CancellationToken cancellationToken)
        {
            // Simple check so we don't save an empty lesson.
            if (string.IsNullOrWhiteSpace(request.LessonTitle))
            {
                return new CreateLessonResponse
                {
                    Success = false,
                    Message = "Lesson title is required."
                };
            }

            // Build the new lesson.
            var lesson = new Lesson
            {
                ModuleId = request.ModuleId,
                LessonTitle = request.LessonTitle,
                Description = request.Description,
                SortOrder = request.SortOrder,
                IsActive = true
            };

            // Save it and get the new LessonId.
            var newLessonId = await _lessonRepository.AddLessonAsync(
                lesson,
                cancellationToken);

            return new CreateLessonResponse
            {
                Success = true,
                Message = "Lesson created successfully.",
                LessonId = newLessonId
            };
        }
    }
}