using MediatR;

namespace RestaurantTraining.Application.Features.Profile.Queries.GetProfile
{
    // Returns null when the user is not found (controller maps null to 404).
    public class GetProfileQuery : IRequest<ProfileDto?>
    {
        public int UserId { get; set; }
    }
}
