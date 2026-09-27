using MediatR;

namespace RestaurantTraining.Application.Features.ResourceQuestions.Queries.GetModulePoints
{
    // Points the learner has earned from MCQs within a single module.
    public class GetModulePointsQuery : IRequest<int>
    {
        public int UserId { get; set; }
        public int ModuleId { get; set; }
    }
}
