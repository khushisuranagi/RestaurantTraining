using MediatR;

namespace RestaurantTraining.Application.Features.ContentCreatorPeople.Queries.GetLearnerProfile
{
    // Returns null when the user is not found (controller maps null to 404).
    public class GetLearnerProfileQuery : IRequest<LearnerProfileDto?>
    {
        public int UserId { get; set; }
    }

    public class LearnerProfileDto
    {
        public int UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}