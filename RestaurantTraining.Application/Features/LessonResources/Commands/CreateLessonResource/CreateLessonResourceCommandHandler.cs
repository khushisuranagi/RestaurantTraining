using MediatR;
using RestaurantTraining.Application.Common.Interfaces;
using RestaurantTraining.Domain.Entities;

namespace RestaurantTraining.Application.Features.LessonResources.Commands.CreateLessonResource
{
    public class CreateLessonResourceCommandHandler
        : IRequestHandler<CreateLessonResourceCommand, CreateLessonResourceResponse>
    {
        private readonly ILessonResourceRepository _lessonResourceRepository;

        public CreateLessonResourceCommandHandler(
            ILessonResourceRepository lessonResourceRepository)
        {
            _lessonResourceRepository = lessonResourceRepository;
        }

        public async Task<CreateLessonResourceResponse> Handle(
            CreateLessonResourceCommand request,
            CancellationToken cancellationToken)
        {
            // A lesson can have only one resource.
            var alreadyHasResource =
                await _lessonResourceRepository.LessonHasResourceAsync(
                    request.LessonId,
                    cancellationToken);

            if (alreadyHasResource)
            {
                return new CreateLessonResourceResponse
                {
                    Success = false,
                    Message =
                        "This lesson already has a resource. " +
                        "Only one resource is allowed per lesson."
                };
            }

            var resource = new LessonResource
            {
                LessonId = request.LessonId,
                ResourceType = request.ResourceType,

                // Used for external resources such as YouTube/articles.
                ResourceUrl = request.ResourceUrl,

                // Used for uploaded Image/PDF files.
                FileData = request.FileData,
                FileName = request.FileName,
                ContentType = request.ContentType,

                ContentText = request.ContentText,
                SortOrder = request.SortOrder,
                IsActive = true
            };

            var newResourceId =
                await _lessonResourceRepository.AddLessonResourceAsync(
                    resource,
                    cancellationToken);

            return new CreateLessonResourceResponse
            {
                Success = true,
                Message = "Lesson resource created successfully.",
                ResourceId = newResourceId
            };
        }
    }
}