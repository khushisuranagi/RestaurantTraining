using MediatR;
using RestaurantTraining.Application.Common.Interfaces;

namespace RestaurantTraining.Application.Features.ContentCreatorDashboard.Queries.GetContentCreatorDashboard
{
    public class GetContentCreatorDashboardQueryHandler
        : IRequestHandler<GetContentCreatorDashboardQuery, ContentCreatorDashboardDto>
    {
        private readonly IContentCreatorDashboardRepository _repository;

        public GetContentCreatorDashboardQueryHandler(
            IContentCreatorDashboardRepository repository)
        {
            _repository = repository;
        }

        public async Task<ContentCreatorDashboardDto> Handle(
            GetContentCreatorDashboardQuery request,
            CancellationToken cancellationToken)
        {
            var modules = await _repository.GetModulesAsync(cancellationToken);
            var activeModules = modules.Where(m => m.IsActive).ToList();

            var activeLessons = await _repository.GetActiveLessonsAsync(cancellationToken);

            var quizQuestionModuleIds = await _repository.GetQuizQuestionModuleIdsAsync(cancellationToken);
            var assignedModuleIds = (await _repository.GetAssignedModuleIdsAsync(cancellationToken)).ToHashSet();
            var assignedRoleIds = (await _repository.GetAssignedRoleIdsAsync(cancellationToken)).ToHashSet();
            var userRoleIds = await _repository.GetAllUserRoleIdsAsync(cancellationToken);
            var lessonIdsWithResources = (await _repository.GetLessonIdsWithResourcesAsync(cancellationToken)).ToHashSet();

            var certificatesIssued = await _repository.GetCertificateCountAsync(cancellationToken);
            var learnersCompleted = await _repository.GetDistinctCertificateLearnerCountAsync(cancellationToken);
            var activeLearners = await _repository.GetActiveLearnerCountAsync(cancellationToken);
            var recentCertificates = await _repository.GetRecentCertificatesAsync(5, cancellationToken);

            // Derived sets
            var moduleIdsWithLessons = activeLessons.Select(l => l.ModuleId).Distinct().ToHashSet();
            var moduleIdsWithQuiz = quizQuestionModuleIds.Distinct().ToHashSet();

            // People whose role has at least one assigned module.
            var learnersAssigned = userRoleIds.Count(r => assignedRoleIds.Contains(r));

            return new ContentCreatorDashboardDto
            {
                ModuleCount = modules.Count,
                ActiveModules = activeModules.Count,
                InactiveModules = modules.Count - activeModules.Count,
                LessonCount = activeLessons.Count,
                QuizQuestionCount = quizQuestionModuleIds.Count,   // one entry per question
                CertificatesIssued = certificatesIssued,
                ModulesAssignedCount = assignedModuleIds.Count,

                LearnersAssigned = learnersAssigned,
                ActiveLearners = activeLearners,
                LearnersCompleted = learnersCompleted,
                RecentCertificates = recentCertificates
                    .Select(c => new RecentCertificateItem
                    {
                        LearnerName = c.LearnerName,
                        ModuleName = c.ModuleName,
                        IssuedDate = c.IssuedDate
                    }).ToList(),

                ModulesWithoutLessons = activeModules
                    .Where(m => !moduleIdsWithLessons.Contains(m.ModuleId))
                    .Select(m => new NamedModuleItem { ModuleId = m.ModuleId, ModuleName = m.ModuleName })
                    .ToList(),

                ModulesWithoutQuiz = activeModules
                    .Where(m => !moduleIdsWithQuiz.Contains(m.ModuleId))
                    .Select(m => new NamedModuleItem { ModuleId = m.ModuleId, ModuleName = m.ModuleName })
                    .ToList(),

                ModulesNotAssigned = activeModules
                    .Where(m => !assignedModuleIds.Contains(m.ModuleId))
                    .Select(m => new NamedModuleItem { ModuleId = m.ModuleId, ModuleName = m.ModuleName })
                    .ToList(),

                LessonsWithoutResources = activeLessons
                    .Where(l => !lessonIdsWithResources.Contains(l.LessonId))
                    .Select(l => new NamedLessonItem { LessonId = l.LessonId, LessonTitle = l.LessonTitle })
                    .ToList(),

                RecentModules = modules
                    .OrderByDescending(m => m.CreatedAt)
                    .Take(5)
                    .Select(m => new RecentModuleItem
                    {
                        ModuleId = m.ModuleId,
                        ModuleName = m.ModuleName,
                        Description = m.Description,
                        IsActive = m.IsActive
                    }).ToList()
            };
        }
    }
}
