using MediatR;
using RestaurantTraining.Application.Common.Interfaces;

namespace RestaurantTraining.Application.Features.LearnerExplore.Queries.GetExploreModule
{
    public class GetExploreModuleQueryHandler
        : IRequestHandler<GetExploreModuleQuery, ExploreModuleContentDto?>
    {
        private readonly ILearnerExploreRepository _repository;

        public GetExploreModuleQueryHandler(
            ILearnerExploreRepository repository)
        {
            _repository = repository;
        }

        public async Task<ExploreModuleContentDto?> Handle(
            GetExploreModuleQuery request,
            CancellationToken cancellationToken)
        {
            var module = await _repository.GetModuleContentAsync(
                request.ModuleId, cancellationToken);

            if (module is null)
                return null;

            return new ExploreModuleContentDto
            {
                ModuleId = module.ModuleId,
                ModuleName = module.ModuleName,
                Description = module.Description,
                Lessons = module.Lessons
                    .Select(lesson => new ExploreLessonDto
                    {
                        LessonId = lesson.LessonId,
                        LessonTitle = lesson.LessonTitle,
                        Description = lesson.Description,
                        SortOrder = lesson.SortOrder,
                        IsCompleted = lesson.IsCompleted,
                        Resources = lesson.Resources
                            .Select(resource => new ExploreResourceDto
                            {
                                ResourceId = resource.ResourceId,
                                ResourceType = resource.ResourceType,
                                ResourceUrl = resource.ResourceUrl,
                                FileData = resource.FileData,
                                FileName = resource.FileName,
                                ContentType = resource.ContentType,
                                ContentText = resource.ContentText,
                                SortOrder = resource.SortOrder
                            })
                            .ToList()
                    })
                    .ToList()
            };
        }
    }
}
