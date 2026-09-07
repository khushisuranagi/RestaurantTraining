using MediatR;

namespace RestaurantTraining.Application.Features.LearnerDashboard.Queries.GetMyModulesOverview
{
    public class GetMyModulesOverviewQuery : IRequest<List<ModuleOverviewDto>>
    {
        public int UserId { get; set; }
    }
}
