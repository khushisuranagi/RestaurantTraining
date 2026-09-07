using MediatR;
using RestaurantTraining.Application.Common.Responses;

// A request/message saying: "I want to delete a Module."
namespace RestaurantTraining.Application.Features.Modules.Commands.DeleteModule
{
    public class DeleteModuleCommand : IRequest<BaseResponse>
    {
        public int ModuleId { get; set; }
    }
}
