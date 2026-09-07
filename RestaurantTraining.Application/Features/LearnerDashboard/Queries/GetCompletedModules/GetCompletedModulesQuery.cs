using MediatR;

namespace RestaurantTraining.Application.Features.LearnerDashboard.Queries.GetCompletedModules
{
    public class GetCompletedModulesQuery : IRequest<List<CompletedModuleDto>>
    {
        public int UserId { get; set; }
    }
}
