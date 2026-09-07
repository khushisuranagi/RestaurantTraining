using MediatR;

namespace RestaurantTraining.Application.Features.LearnerDashboard.Commands.StartModule
{
    public class StartModuleCommand : IRequest<StartModuleResultDto>
    {
        public int UserId { get; set; }
        public int ModuleId { get; set; }
    }
}
