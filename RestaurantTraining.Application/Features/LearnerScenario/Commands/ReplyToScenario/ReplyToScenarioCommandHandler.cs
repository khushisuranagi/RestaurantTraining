using MediatR;
using RestaurantTraining.Application.Common.Interfaces;

namespace RestaurantTraining.Application.Features.LearnerScenario.Commands.ReplyToScenario
{
    public class ReplyToScenarioCommandHandler
        : IRequestHandler<ReplyToScenarioCommand, ReplyToScenarioResult>
    {
        private readonly IAIScenarioRepository _repository;
        private readonly IAiRoleplay _roleplay;
        private readonly ICertificateIssuanceRepository _certificates;

        // The customer must be satisfied within this many learner messages.
        private const int MaxLearnerMessages = 4;

        public ReplyToScenarioCommandHandler(
            IAIScenarioRepository repository,
            IAiRoleplay roleplay,
            ICertificateIssuanceRepository certificates)
        {
            _repository = repository;
            _roleplay = roleplay;
            _certificates = certificates;
        }

        public async Task<ReplyToScenarioResult> Handle(
            ReplyToScenarioCommand request, CancellationToken cancellationToken)
        {
            var scenario = await _repository.GetScenarioByIdAsync(
                request.ScenarioId, cancellationToken);

            if (scenario is null)
                return new ReplyToScenarioResult { NotAvailable = true };

            // Ask the AI customer to reply + judge
            var turn = await _roleplay.ContinueAsync(new RoleplayContext
            {
                ScenarioPrompt = scenario.ScenarioPrompt,
                Messages = request.Messages
                    .Select(m => new RoleplayMessage { FromLearner = m.FromLearner, Text = m.Text })
                    .ToList()
            }, cancellationToken);

            if (turn is null)
                return new ReplyToScenarioResult { NotAvailable = true };

            var learnerMessageCount = request.Messages.Count(m => m.FromLearner);
            var reachedCap = learnerMessageCount >= MaxLearnerMessages;

            var passed = turn.Satisfied;
            var ended = passed || reachedCap;

            var result = new ReplyToScenarioResult
            {
                Reply = turn.Reply,
                Feedback = turn.Feedback,
                Ended = ended,
                Passed = passed
            };

            if (!ended)
                return result;   // conversation continues

            // Record the attempt (pass or fail).
            await _repository.RecordAttemptAsync(
                request.UserId, scenario.ScenarioId, passed, turn.Feedback, cancellationToken);

            // On a pass, issue the certificate IF the quiz is also passed and
            // one hasn't been issued yet.
            if (passed)
            {
                var quizPassed = await _certificates.HasPassedModuleQuizAsync(
                    request.UserId, scenario.ModuleId, cancellationToken);

                var hasCert = await _certificates.HasCertificateAsync(
                    request.UserId, scenario.ModuleId, cancellationToken);

                if (quizPassed && !hasCert)
                {
                    var number =
                        $"CERT-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..8].ToUpperInvariant()}";

                    await _certificates.IssueCertificateAsync(
                        request.UserId, scenario.ModuleId, number, cancellationToken);

                    result.CertificateIssued = true;
                }
            }

            return result;
        }
    }
}
