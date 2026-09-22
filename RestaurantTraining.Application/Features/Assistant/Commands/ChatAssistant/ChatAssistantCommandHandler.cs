using System.Text;
using MediatR;
using RestaurantTraining.Application.Common.Interfaces;

namespace RestaurantTraining.Application.Features.Assistant.Commands.ChatAssistant
{
    public class ChatAssistantCommandHandler
        : IRequestHandler<ChatAssistantCommand, ChatAssistantResult>
    {
        private readonly IAiAssistant _assistant;
        private readonly ILearnerDashboardRepository _learnerRepository;
        private readonly IContentCreatorDashboardRepository _creatorRepository;

        public ChatAssistantCommandHandler(
            IAiAssistant assistant,
            ILearnerDashboardRepository learnerRepository,
            IContentCreatorDashboardRepository creatorRepository)
        {
            _assistant = assistant;
            _learnerRepository = learnerRepository;
            _creatorRepository = creatorRepository;
        }

        public async Task<ChatAssistantResult> Handle(
            ChatAssistantCommand request, CancellationToken cancellationToken)
        {
            var dataContext = request.RoleName == "Content Creator"
                ? await BuildCreatorContextAsync(cancellationToken)
                : await BuildProgressContextAsync(request, cancellationToken);

            var systemContext =
                "You are a friendly, concise help assistant inside 'Copperleaf', a web app for " +
                "restaurant staff training. " +
                (string.IsNullOrWhiteSpace(request.RoleName) ? "" : $"The user is a '{request.RoleName}'. ") +
                "Help them with how to use the app and with restaurant training topics " +
                "(customer service, food safety, hygiene, etc.). Keep answers short and practical. " +
                "If a question is unrelated to the app or restaurant training, gently steer back." +
                dataContext;

            var messages = request.Messages
                .Select(m => new AssistantMessage { FromUser = m.FromUser, Text = m.Text })
                .ToList();

            var reply = await _assistant.ReplyAsync(systemContext, messages, cancellationToken);

            return new ChatAssistantResult
            {
                Reply = string.IsNullOrWhiteSpace(reply)
                    ? "Sorry, I couldn't respond right now. Please try again."
                    : reply
            };
        }

        // Live numbers for a learner, added to the prompt on every message.
        private async Task<string> BuildProgressContextAsync(
            ChatAssistantCommand request, CancellationToken cancellationToken)
        {
            if (request.UserId <= 0)
                return string.Empty;

            try
            {
                var modules = await _learnerRepository.GetLearnerModuleStatesAsync(
                    request.UserId, cancellationToken);
                var certifiedIds = (await _learnerRepository.GetCertifiedModuleIdsAsync(
                    request.UserId, cancellationToken)).ToHashSet();
                var scenariosPassed = await _learnerRepository.GetPassedScenarioCountAsync(
                    request.UserId, cancellationToken);
                var points = await _learnerRepository.GetResourceQuestionPointsAsync(
                    request.UserId, cancellationToken);

                var completed = modules.Where(m => m.IsCompleted).ToList();
                var inProgress = modules.Where(m => m.HasStarted && !m.IsCompleted).ToList();
                var notStarted = modules.Where(m => !m.HasStarted).ToList();
                var certificatesPending = completed.Count(m => !certifiedIds.Contains(m.ModuleId));

                var sb = new StringBuilder();
                sb.AppendLine();
                sb.AppendLine();
                sb.AppendLine("THE USER'S CURRENT TRAINING DATA (live from the database):");
                sb.AppendLine($"- Modules assigned to them: {modules.Count}");
                sb.AppendLine($"- Completed: {completed.Count}");
                sb.AppendLine($"- In progress (started, not finished): {inProgress.Count}");
                sb.AppendLine($"- Not started yet: {notStarted.Count}");
                sb.AppendLine($"- Certificates earned: {certifiedIds.Count}");
                sb.AppendLine($"- Certificates pending (module completed, certificate not yet issued): {certificatesPending}");
                sb.AppendLine($"- Lessons completed: {modules.Sum(m => m.CompletedLessons)} of {modules.Sum(m => m.TotalLessons)}");
                sb.AppendLine($"- AI practice scenarios passed: {scenariosPassed}");
                sb.AppendLine($"- Total points: {points}");

                AppendModuleList(sb, "In-progress modules", inProgress, showLessons: true);
                AppendModuleList(sb, "Not-started modules", notStarted, showLessons: false);
                AppendModuleList(sb, "Completed modules", completed, showLessons: false);

                sb.AppendLine();
                sb.AppendLine(
                    "When the user asks about their own progress or modules (how many are in progress, " +
                    "assigned, completed, remaining, certificates, points), answer directly using these " +
                    "numbers, for example: 'You currently have 2 modules in progress.' Do not tell them " +
                    "to go and check the dashboard for information that is listed above. Use only these " +
                    "figures and never invent numbers. If they ask for something not listed here, say " +
                    "you don't have that detail and suggest the dashboard or Learning Modules page. " +
                    "Reply in plain text only, with no Markdown formatting: no asterisks for bold or italics, " +
                    "no backticks, no headings, no bullet characters like '-' or '*'. Use plain sentences " +
                    "and, if you need a list, write it as short numbered sentences instead.");

                return sb.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        // Org-wide numbers for a Content Creator, added to the prompt on every message.
        private async Task<string> BuildCreatorContextAsync(CancellationToken cancellationToken)
        {
            try
            {
                var modules = await _creatorRepository.GetModulesAsync(cancellationToken);
                var activeModules = modules.Where(m => m.IsActive).ToList();
                var activeLessons = await _creatorRepository.GetActiveLessonsAsync(cancellationToken);
                var quizQuestionModuleIds = await _creatorRepository.GetQuizQuestionModuleIdsAsync(cancellationToken);
                var assignedModuleIds = (await _creatorRepository.GetAssignedModuleIdsAsync(cancellationToken)).ToHashSet();
                var lessonIdsWithResources = (await _creatorRepository.GetLessonIdsWithResourcesAsync(cancellationToken)).ToHashSet();

                var certificatesIssued = await _creatorRepository.GetCertificateCountAsync(cancellationToken);
                var learnersCompleted = await _creatorRepository.GetDistinctCertificateLearnerCountAsync(cancellationToken);
                var activeLearners = await _creatorRepository.GetActiveLearnerCountAsync(cancellationToken);

                var moduleIdsWithLessons = activeLessons.Select(l => l.ModuleId).Distinct().ToHashSet();
                var moduleIdsWithQuiz = quizQuestionModuleIds.Distinct().ToHashSet();

                var modulesWithoutLessons = activeModules.Where(m => !moduleIdsWithLessons.Contains(m.ModuleId)).ToList();
                var modulesWithoutQuiz = activeModules.Where(m => !moduleIdsWithQuiz.Contains(m.ModuleId)).ToList();
                var modulesNotAssigned = activeModules.Where(m => !assignedModuleIds.Contains(m.ModuleId)).ToList();
                var lessonsWithoutResources = activeLessons.Where(l => !lessonIdsWithResources.Contains(l.LessonId)).ToList();

                var sb = new StringBuilder();
                sb.AppendLine();
                sb.AppendLine();
                sb.AppendLine("ORGANIZATION TRAINING DATA (live from the database):");
                sb.AppendLine($"- Total modules: {modules.Count} ({activeModules.Count} active, {modules.Count - activeModules.Count} inactive)");
                sb.AppendLine($"- Total lessons (active): {activeLessons.Count}");
                sb.AppendLine($"- Modules assigned to a role: {assignedModuleIds.Count}");
                sb.AppendLine($"- Certificates issued: {certificatesIssued}");
                sb.AppendLine($"- Learners who completed at least one module: {learnersCompleted}");
                sb.AppendLine($"- Active learners (have made progress): {activeLearners}");
                sb.AppendLine($"- Active modules with no lessons: {modulesWithoutLessons.Count}");
                sb.AppendLine($"- Active modules with no quiz: {modulesWithoutQuiz.Count}");
                sb.AppendLine($"- Active modules not assigned to any role: {modulesNotAssigned.Count}");
                sb.AppendLine($"- Lessons with no material/resources: {lessonsWithoutResources.Count}");

                AppendNamedList(sb, "Modules with no lessons", modulesWithoutLessons.Select(m => m.ModuleName));
                AppendNamedList(sb, "Modules with no quiz", modulesWithoutQuiz.Select(m => m.ModuleName));
                AppendNamedList(sb, "Modules not assigned to any role", modulesNotAssigned.Select(m => m.ModuleName));
                AppendNamedList(sb, "Lessons with no material", lessonsWithoutResources.Select(l => l.LessonTitle));

                sb.AppendLine();
                sb.AppendLine(
                    "When the user (a Content Creator) asks about the state of their training content or " +
                    "learners (which modules lack lessons or a quiz, which lessons lack material, how many " +
                    "certificates have been issued, how many active learners there are, etc.), answer " +
                    "directly using these numbers and names, for example: 'Two modules have no lessons: " +
                    "X and Y.' Do not tell them to go and check the dashboard for information listed above. " +
                    "Use only these figures and never invent numbers or names. If they ask for something not " +
                    "listed here, say you don't have that detail and suggest the dashboard. " +
                    "Reply in plain text only, with no Markdown formatting: no asterisks for bold or italics, " +
                    "no backticks, no headings, no bullet characters like '-' or '*'. Use plain sentences " +
                    "and, if you need a list, write it as short numbered sentences instead.");

                return sb.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        private static void AppendModuleList(
            StringBuilder sb,
            string title,
            List<LearnerModuleStateInfo> modules,
            bool showLessons)
        {
            if (modules.Count == 0) return;

            sb.AppendLine($"{title}:");
            foreach (var m in modules.Take(20))
            {
                if (showLessons)
                {
                    var percent = (int)Math.Round(
                        100.0 * (m.CompletedLessons + (m.QuizPassed ? 1 : 0))
                        / (m.TotalLessons + 1));

                    sb.AppendLine(
                        $"  * {m.ModuleName} - {percent}% complete " +
                        $"({m.CompletedLessons} of {m.TotalLessons} lessons done, " +
                        $"quiz {(m.QuizPassed ? "passed" : "not passed yet")})");
                }
                else
                {
                    sb.AppendLine($"  * {m.ModuleName}");
                }
            }
        }

        private static void AppendNamedList(StringBuilder sb, string title, IEnumerable<string> names)
        {
            var list = names.Take(20).ToList();
            if (list.Count == 0) return;

            sb.AppendLine($"{title}:");
            foreach (var name in list)
                sb.AppendLine($"  * {name}");
        }
    }
}