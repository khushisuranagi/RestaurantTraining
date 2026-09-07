using MediatR;

namespace RestaurantTraining.Application.Features.LearnerDashboard.Queries.GetExploreModules
{
    public class GetExploreModulesQuery : IRequest<List<ExploreModuleDto>>
    {
        public int UserId { get; set; }
    }
}
