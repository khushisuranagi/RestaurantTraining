using MediatR;
using RestaurantTraining.Application.Common.Interfaces;
using RestaurantTraining.Application.Common.Responses;

namespace RestaurantTraining.Application.Features.Lessons.Commands.DeleteLesson
{
    public class DeleteLessonCommandHandler
        : IRequestHandler<DeleteLessonCommand, BaseResponse>
    {
        private readonly ILessonRepository _lessonRepository;

        public DeleteLessonCommandHandler(
            ILessonRepository lessonRepository)
        {
            _lessonRepository = lessonRepository;
        }

        public async Task<BaseResponse> Handle(
            DeleteLessonCommand request,
            CancellationToken cancellationToken)
        {
            var lesson = await _lessonRepository.GetLessonByIdAsync(
                request.LessonId,
                cancellationToken);

            if (lesson == null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Lesson not found."
                };
            }

            await _lessonRepository.DeleteLessonAsync(
                lesson,
                cancellationToken);

            return new BaseResponse
            {
                Success = true,
                Message = "Lesson deleted successfully."
            };
        }
    }
}