using MediatR;
using RestaurantTraining.Application.Common.Interfaces;

namespace RestaurantTraining.Application.Features.ResourceQuestions.Commands.SubmitResourceAnswer
{
    public class SubmitResourceAnswerCommandHandler
        : IRequestHandler<SubmitResourceAnswerCommand, SubmitResourceAnswerResult>
    {
        private readonly IResourceQuestionRepository _repository;

        public SubmitResourceAnswerCommandHandler(
            IResourceQuestionRepository repository)
        {
            _repository = repository;
        }

        // Points given for a correct answer (first time only).
        private const int PointsPerCorrectAnswer = 10;

        public async Task<SubmitResourceAnswerResult> Handle(
            SubmitResourceAnswerCommand request,
            CancellationToken cancellationToken)
        {
            var stored = await _repository.GetByResourceIdAsync(
                request.ResourceId, cancellationToken);

            if (stored is null)
                return new SubmitResourceAnswerResult { HasQuestion = false };

            // Grade on the server — the correct flag never leaves this layer.
            var isCorrect = stored.Options
                .Any(o => o.OptionId == request.SelectedOptionId && o.IsCorrect);

            var pointsAwarded = 0;
            if (isCorrect)
            {
                // Records the pass + awards points, but only the first time.
                pointsAwarded = await _repository.RecordCorrectAnswerAsync(
                    request.UserId, request.ResourceId, PointsPerCorrectAnswer, cancellationToken);
            }

            var totalPoints = await _repository.GetTotalPointsAsync(
                request.UserId, cancellationToken);

            return new SubmitResourceAnswerResult
            {
                HasQuestion = true,
                IsCorrect = isCorrect,
                Explanation = stored.Explanation,
                PointsAwarded = pointsAwarded,
                TotalPoints = totalPoints
            };
        }
    }
}
