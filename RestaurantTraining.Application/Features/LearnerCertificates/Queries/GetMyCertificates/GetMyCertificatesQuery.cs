using MediatR;

namespace RestaurantTraining.Application.Features.LearnerCertificates.Queries.GetMyCertificates
{
    // Read request: "give me this learner's certificates".
    // The controller reads the signed-in user's id from the JWT and passes it in.
    public class GetMyCertificatesQuery : IRequest<List<LearnerCertificateDto>>
    {
        public int UserId { get; set; }
    }
}
