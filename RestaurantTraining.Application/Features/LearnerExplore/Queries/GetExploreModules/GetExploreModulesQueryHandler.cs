using MediatR;
using RestaurantTraining.Application.Common.Interfaces;

namespace RestaurantTraining.Application.Features.LearnerExplore.Queries.GetExploreModules
{
    public class GetExploreModulesQueryHandler
        : IRequestHandler<GetExploreModulesQuery, List<ExploreModuleDto>>
    {
        private readonly ILearnerExploreRepository _repository;

        public GetExploreModulesQueryHandler(
            ILearnerExploreRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ExploreModuleDto>> Handle(
            GetExploreModulesQuery request,
            CancellationToken cancellationToken)
        {
            var modules = await _repository.GetUnassignedModulesAsync(
                request.RoleName, request.UserId, cancellationToken);

            return modules
                .Select(m => new ExploreModuleDto
                {
                    ModuleId = m.ModuleId,
                    ModuleName = m.ModuleName,
                    Description = m.Description,
                    CoverImageData = m.CoverImageData,
                    CoverImageContentType = m.CoverImageContentType
                })
                .ToList();
        }
    }
}
