using MediatR;
using RestaurantTraining.Application.Common.Responses;

// A request/message saying: "I want to delete a Module."
namespace RestaurantTraining.Application.Features.Modules.Commands.DeleteModule
{
    public class DeleteModuleCommand : IRequest<DeleteModuleResponse>
    {
        public int ModuleId { get; set; }
        public bool ConfirmCascade { get; set; }
    }
}
