using MediatR;
using RestaurantTraining.Application.Common.Interfaces;

namespace RestaurantTraining.Application.Features.LearnerDashboard.Queries.GetLearnerDashboard
{
    public class GetLearnerDashboardQueryHandler
        : IRequestHandler<GetLearnerDashboardQuery, LearnerDashboardDto>
    {
        private readonly ILearnerDashboardRepository _repository;

        public GetLearnerDashboardQueryHandler(
            ILearnerDashboardRepository repository)
        {
            _repository = repository;
        }

        public async Task<LearnerDashboardDto> Handle(
            GetLearnerDashboardQuery request,
            CancellationToken cancellationToken)
        {
            var modules = await _repository.GetLearnerModuleStatesAsync(
                request.UserId, cancellationToken);

            var moduleIds = modules.Select(m => m.ModuleId).ToList();

            var totalQuizzes = await _repository.GetDistinctQuizModuleCountAsync(
                moduleIds, cancellationToken);

            var quizzesAttempted = await _repository.GetAttemptedQuizModuleCountAsync(
                request.UserId, moduleIds, cancellationToken);

            var certifiedModuleIds = (await _repository.GetCertifiedModuleIdsAsync(
                request.UserId, cancellationToken)).ToHashSet();

            var certificatesEarned = certifiedModuleIds.Count;
            var certificatesPending = modules
                .Count(m => m.IsCompleted && !certifiedModuleIds.Contains(m.ModuleId));

            var aiScenariosCompleted = await _repository.GetPassedScenarioCountAsync(
                request.UserId, cancellationToken);

            return new LearnerDashboardDto
            {
                LessonsCompleted = modules.Sum(x => x.CompletedLessons),
                TotalLessons = modules.Sum(x => x.TotalLessons),

                ModulesCompleted = modules.Count(x => x.IsCompleted),
                TotalModules = modules.Count,

                QuizzesAttempted = quizzesAttempted,
                TotalQuizzes = totalQuizzes,

                CertificatesEarned = certificatesEarned,
                CertificatesPending = certificatesPending,

                AiScenariosCompleted = aiScenariosCompleted,

                PendingModules = modules
                    .Where(x => x.HasStarted && !x.IsCompleted)
                    .OrderByDescending(x => x.LastAccessedAt)
                    .Select(x => new PendingModuleDto
                    {
                        ModuleId = x.ModuleId,
                        ModuleName = x.ModuleName,
                        Description = x.Description,
                        CompletedLessons = x.CompletedLessons,
                        TotalLessons = x.TotalLessons,
                        // The quiz counts as one final step, so a module can only
                        // reach 100% once every lesson is done AND the quiz is passed.
                        ProgressPercent = (int)Math.Round(
                            100.0 * (x.CompletedLessons + (x.QuizPassed ? 1 : 0))
                            / (x.TotalLessons + 1)),
                        IsCompleted = x.IsCompleted
                    })
                    .ToList()
            };
        }
    }
}
