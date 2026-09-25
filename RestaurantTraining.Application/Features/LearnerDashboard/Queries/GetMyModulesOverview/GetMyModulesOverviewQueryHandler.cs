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
                    QuizPassed = x.QuizPassed,
                    CompletedLessons = x.CompletedLessons,
                    TotalLessons = x.TotalLessons,
                    // Progress has three stages: lessons, then the quiz, then the
                    // practice scenario (denominator = lessons + quiz + scenario).
                    // A module only reaches 100% once its certificate is issued
                    // (IsCompleted), i.e. the scenario is passed too.
                    ProgressPercent = x.IsCompleted
                        ? 100
                        : (int)Math.Round(
                            100.0 * (x.CompletedLessons + (x.QuizPassed ? 1 : 0))
                            / (x.TotalLessons + 2)),
                    CoverImageData = x.CoverImageData,
                    CoverImageContentType = x.CoverImageContentType












































                })
                .ToList();
        }
    }
}
