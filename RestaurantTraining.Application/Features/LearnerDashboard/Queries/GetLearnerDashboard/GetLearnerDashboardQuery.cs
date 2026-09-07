using MediatR;

namespace RestaurantTraining.Application.Features.LearnerDashboard.Queries.GetLearnerDashboard
{
    public class GetLearnerDashboardQuery : IRequest<LearnerDashboardDto>
    {
        public int UserId { get; set; }
    }
}
