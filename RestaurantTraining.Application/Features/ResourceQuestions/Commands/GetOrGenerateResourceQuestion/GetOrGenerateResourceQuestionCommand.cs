using MediatR;

namespace RestaurantTraining.Application.Features.ResourceQuestions.Commands.GetOrGenerateResourceQuestion
{
    // "Give me the MCQ for this resource — generate + store it if it doesn't
    // exist yet, otherwise return the stored one."
    public class GetOrGenerateResourceQuestionCommand : IRequest<ResourceQuestionResult>
    {
        public int ResourceId { get; set; }

        // Set by the controller from the JWT — used to tell whether this learner
        // has already answered the question correctly.
        public int UserId { get; set; }
    }
}
