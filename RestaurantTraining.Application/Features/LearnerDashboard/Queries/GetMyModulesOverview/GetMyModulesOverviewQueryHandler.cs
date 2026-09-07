using MediatR;
using RestaurantTraining.Application.Common.Interfaces;

namespace RestaurantTraining.Application.Features.LearnerDashboard.Queries.GetMyModulesOverview
{
    public class GetMyModulesOverviewQueryHandler
        : IRequestHandler<GetMyModulesOverviewQuery, List<ModuleOverviewDto>>
    {
        private readonly ILearnerDashboardRepository _repository;

        public GetMyModulesOverviewQueryHandler(
            ILearnerDashboardRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ModuleOverviewDto>> Handle(
            GetMyModulesOverviewQuery request,
            CancellationToken cancellationToken)
        {
            var modules = await _repository.GetLearnerModuleStatesAsync(
                request.UserId, cancellationToken);

            return modules
                .OrderByDescending(x => x.HasStarted)
                .ThenByDescending(x => x.AssignedAt)
                .Select(x => new ModuleOverviewDto
                {
                    ModuleId = x.ModuleId,
                    ModuleName = x.ModuleName,
                    Description = x.Description,
                    HasStarted = x.HasStarted,
                    IsCompleted = x.IsCompleted,
                    CompletedLessons = x.CompletedLessons,
                    TotalLessons = x.TotalLessons,
                    ProgressPercent = (int)Math.Round(
                        100.0 * (x.CompletedLessons + (x.QuizPassed ? 1 : 0))
                        / (x.TotalLessons + 1))
                })
                .ToList();
        }
    }
}
