using MediatR;
using RestaurantTraining.Application.Common.Interfaces;

namespace RestaurantTraining.Application.Features.LessonResources.Commands.UpdateLessonResource
{
    public class UpdateLessonResourceCommandHandler
        : IRequestHandler<UpdateLessonResourceCommand, UpdateLessonResourceResponse>
    {
        private readonly ILessonResourceRepository _lessonResourceRepository;

        public UpdateLessonResourceCommandHandler(
            ILessonResourceRepository lessonResourceRepository)
        {
            _lessonResourceRepository = lessonResourceRepository;
        }

        public async Task<UpdateLessonResourceResponse> Handle(
            UpdateLessonResourceCommand request,
            CancellationToken cancellationToken)
        {
            var resource =
                await _lessonResourceRepository.GetLessonResourceByIdAsync(
                    request.ResourceId,
                    cancellationToken);

            if (resource == null)
            {
                return new UpdateLessonResourceResponse
                {
                    Success = false,
                    Message = "Lesson resource not found."
                };
            }

            resource.LessonId = request.LessonId;
            resource.ResourceType = request.ResourceType;
            resource.ResourceUrl = request.ResourceUrl;

            // Only replace the stored file when new file data is supplied.
            if (!string.IsNullOrWhiteSpace(request.FileData))
            {
                resource.FileData = request.FileData;
                resource.FileName = request.FileName;
                resource.ContentType = request.ContentType;
            }

            resource.ContentText = request.ContentText;
            resource.SortOrder = request.SortOrder;
            resource.IsActive = request.IsActive;

            await _lessonResourceRepository.UpdateLessonResourceAsync(
                resource,
                cancellationToken);

            return new UpdateLessonResourceResponse
            {
                Success = true,
                Message = "Lesson resource updated successfully.",
                ResourceId = resource.ResourceId
            };
        }
    }
}