using MediatR;
using RestaurantTraining.Application.Common.Interfaces;

namespace RestaurantTraining.Application.Features.LearnerDashboard.Queries.GetInProgressModules
{
    public class GetInProgressModulesQueryHandler
        : IRequestHandler<GetInProgressModulesQuery, List<InProgressModuleDto>>
    {
        private readonly ILearnerDashboardRepository _repository;

        public GetInProgressModulesQueryHandler(
            ILearnerDashboardRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<InProgressModuleDto>> Handle(
            GetInProgressModulesQuery request,
            CancellationToken cancellationToken)
        {
            var modules = await _repository.GetLearnerModuleStatesAsync(
                request.UserId, cancellationToken);

            return modules
                .Where(x => x.HasStarted && !x.IsCompleted)
                .OrderByDescending(x => x.LastAccessedAt)
                .Select(x => new InProgressModuleDto
                {
                    ModuleId = x.ModuleId,
                    ModuleName = x.ModuleName,
                    Description = x.Description,
                    CompletedLessons = x.CompletedLessons,
                    TotalLessons = x.TotalLessons,
                    LastAccessedAt = x.LastAccessedAt
                })
                .ToList();
        }
    }
}
