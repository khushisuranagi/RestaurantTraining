using MediatR;

namespace RestaurantTraining.Application.Features.ContentCreatorPeople.Commands.SetLearnerActiveStatus
{
    public class SetLearnerActiveStatusCommand : IRequest<SetLearnerActiveStatusResult>
    {
        // Set by the controller from the route, never trusted from the body.
        public int UserId { get; set; }

        public bool IsActive { get; set; }
    }

    public class SetLearnerActiveStatusResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}