using MediatR;

namespace RestaurantTraining.Application.Features.LearnerQuiz.Queries.GetQuiz
{
    // Returns null when the module is not assigned to the learner's role
    // (the controller turns null into a 404 Not Found).
    public class GetQuizQuery : IRequest<LearnerQuizDto?>
    {
        public string RoleName { get; set; } = string.Empty;
        public int ModuleId { get; set; }
    }
}
