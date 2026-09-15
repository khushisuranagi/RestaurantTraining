using MediatR;

namespace RestaurantTraining.Application.Features.LearnerExplore.Queries.GetExploreModules
{
    public class GetExploreModulesQuery : IRequest<List<ExploreModuleDto>>
    {
        public string RoleName { get; set; } = string.Empty;
    }
}
