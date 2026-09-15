using MediatR;

namespace RestaurantTraining.Application.Features.LearnerExplore.Queries.GetExploreModule
{
    // Returns null when the module doesn't exist / isn't active (controller → 404).
    public class GetExploreModuleQuery : IRequest<ExploreModuleContentDto?>
    {
        public int ModuleId { get; set; }
    }
}
