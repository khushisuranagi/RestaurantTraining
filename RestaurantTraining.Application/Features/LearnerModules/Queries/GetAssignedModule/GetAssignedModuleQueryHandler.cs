using MediatR;
using RestaurantTraining.Application.Common.Interfaces;

namespace RestaurantTraining.Application.Features.LearnerModules.Queries.GetAssignedModule
{
    public class GetAssignedModuleQueryHandler
        : IRequestHandler<GetAssignedModuleQuery, ModuleContentDto?>
    {
        private readonly ILearnerModuleRepository _repository;

        public GetAssignedModuleQueryHandler(
            ILearnerModuleRepository repository)
        {
            _repository = repository;
        }

        public async Task<ModuleContentDto?> Handle(
            GetAssignedModuleQuery request,
            CancellationToken cancellationToken)
        {
            var module = await _repository.GetAssignedModuleAsync(
                request.RoleName, request.UserId, request.ModuleId, cancellationToken);

            if (module is null)
                return null;

            return new ModuleContentDto
            {
                ModuleId = module.ModuleId,
                ModuleName = module.ModuleName,
                Description = module.Description,
                Lessons = module.Lessons
                    .Select(lesson => new LessonDto
                    {
                        LessonId = lesson.LessonId,
                        LessonTitle = lesson.LessonTitle,
                        Description = lesson.Description,
                        SortOrder = lesson.SortOrder,
                        IsCompleted = lesson.IsCompleted,
                        Resources = lesson.Resources
                            .Select(resource => new LessonResourceDto
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
