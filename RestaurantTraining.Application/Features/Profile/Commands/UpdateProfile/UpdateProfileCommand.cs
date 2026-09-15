using MediatR;

namespace RestaurantTraining.Application.Features.Profile.Commands.UpdateProfile
{
    public class UpdateProfileCommand : IRequest<UpdateProfileResult>
    {
        // Set by the controller from the JWT (not the request body).
        public int UserId { get; set; }

        // Editable personal info.
        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
