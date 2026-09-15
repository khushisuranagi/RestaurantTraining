using MediatR;

namespace RestaurantTraining.Application.Features.LearnerModules.Commands.RecordModuleOpened
{
    public class RecordModuleOpenedCommand : IRequest
    {
        public int UserId { get; set; }
        public int ModuleId { get; set; }
    }
}