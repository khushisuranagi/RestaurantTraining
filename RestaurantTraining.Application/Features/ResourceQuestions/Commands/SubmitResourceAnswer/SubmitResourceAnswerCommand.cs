using MediatR;

namespace RestaurantTraining.Application.Features.ResourceQuestions.Commands.SubmitResourceAnswer
{
    public class SubmitResourceAnswerCommand : IRequest<SubmitResourceAnswerResult>
    {
        // Set by the controller from the route + JWT.
        public int ResourceId { get; set; }
        public int UserId { get; set; }

        // Bound from the request body.
        public int SelectedOptionId { get; set; }
    }

    public class SubmitResourceAnswerResult
    {
        public bool HasQuestion { get; set; }   // false → controller returns 404
        public bool IsCorrect { get; set; }
        public string Explanation { get; set; } = string.Empty;

        // Points awarded on THIS answer (0 if wrong or already earned).
        public int PointsAwarded { get; set; }

        // The learner's running total of resource-question points.
        public int TotalPoints { get; set; }
    }
}
