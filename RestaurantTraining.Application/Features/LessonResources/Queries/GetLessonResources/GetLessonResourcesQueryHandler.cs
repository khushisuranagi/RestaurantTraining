using MediatR;
using RestaurantTraining.Application.Common.Interfaces;

namespace RestaurantTraining.Application.Features.LessonResources.Queries.GetLessonResources
{
    public class GetLessonResourcesQueryHandler
        : IRequestHandler<GetLessonResourcesQuery, List<LessonResourceDto>>
    {
        private readonly ILessonResourceRepository _lessonResourceRepository;

        public GetLessonResourcesQueryHandler(
            ILessonResourceRepository lessonResourceRepository)
        {
            _lessonResourceRepository = lessonResourceRepository;
        }

        public async Task<List<LessonResourceDto>> Handle(
            GetLessonResourcesQuery request,
            CancellationToken cancellationToken)
        {
            var resources =
                await _lessonResourceRepository.GetAllLessonResourcesAsync(
                    cancellationToken);

            return resources
                .Select(x => new LessonResourceDto
                {
                    ResourceId = x.ResourceId,
                    LessonId = x.LessonId,
                    ResourceType = x.ResourceType,
                    ResourceUrl = x.ResourceUrl,

                    FileData = x.FileData,
                    FileName = x.FileName,
                    ContentType = x.ContentType,

                    ContentText = x.ContentText,
                    SortOrder = x.SortOrder,
                    IsActive = x.IsActive
                })
                .ToList();
        }
    }
}