using MediatR;
using RestaurantTraining.Application.Common.Interfaces;
using RestaurantTraining.Application.Common.Responses;

namespace RestaurantTraining.Application.Features.LessonResources.Commands.DeleteLessonResource
{
    public class DeleteLessonResourceCommandHandler
        : IRequestHandler<DeleteLessonResourceCommand, BaseResponse>
    {
        private readonly ILessonResourceRepository _lessonResourceRepository;

        public DeleteLessonResourceCommandHandler(
            ILessonResourceRepository lessonResourceRepository)
        {
            _lessonResourceRepository = lessonResourceRepository;
        }

        public async Task<BaseResponse> Handle(
            DeleteLessonResourceCommand request,
            CancellationToken cancellationToken)
        {
            var resource =
                await _lessonResourceRepository.GetLessonResourceByIdAsync(
                    request.ResourceId,
                    cancellationToken);

            if (resource == null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Lesson resource not found."
                };
            }

            await _lessonResourceRepository.DeleteLessonResourceAsync(
                resource,
                cancellationToken);

            return new BaseResponse
            {
                Success = true,
                Message = "Lesson resource deleted successfully."
            };
        }
    }
}