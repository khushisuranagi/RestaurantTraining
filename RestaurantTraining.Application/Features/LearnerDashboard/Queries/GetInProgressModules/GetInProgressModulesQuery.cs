using MediatR;

namespace RestaurantTraining.Application.Features.LearnerDashboard.Queries.GetInProgressModules
{
    public class GetInProgressModulesQuery : IRequest<List<InProgressModuleDto>>
    {
        public int UserId { get; set; }
    }
}
