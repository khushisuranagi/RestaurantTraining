using MediatR;
using RestaurantTraining.Application.Common.Interfaces;

namespace RestaurantTraining.Application.Features.LearnerDashboard.Queries.GetExploreModules
{
    public class GetExploreModulesQueryHandler
        : IRequestHandler<GetExploreModulesQuery, List<ExploreModuleDto>>
    {
        private readonly ILearnerDashboardRepository _repository;

        public GetExploreModulesQueryHandler(
            ILearnerDashboardRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ExploreModuleDto>> Handle(
            GetExploreModulesQuery request,
            CancellationToken cancellationToken)
        {
            var modules = await _repository.GetLearnerModuleStatesAsync(
                request.UserId, cancellationToken);

            // Only modules the learner has not started yet.
            return modules
                .Where(x => !x.HasStarted)
                .OrderByDescending(x => x.AssignedAt)
                .Select(x => new ExploreModuleDto
                {
                    ModuleId = x.ModuleId,
                    ModuleName = x.ModuleName,
                    Description = x.Description,
                    AssignedAt = x.AssignedAt
                })
                .ToList();
        }
    }
}
