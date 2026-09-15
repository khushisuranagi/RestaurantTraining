using MediatR;

namespace RestaurantTraining.Application.Features.Auth.Commands.DeleteAccount
{
    public class DeleteAccountCommand : IRequest<DeleteAccountResponse>
    {
        // Set by the controller from the JWT, never the request body.
        public int UserId { get; set; }

        // The user must confirm with their current password.
        public string CurrentPassword { get; set; } = string.Empty;
    }
}
