using MediatR;
using RestaurantTraining.Application.Common.Responses;

//  want to update an existing Module.
namespace RestaurantTraining.Application.Features.Modules.Commands.UpdateModule
{
    public class UpdateModuleCommand : IRequest<BaseResponse>
    {
        public int ModuleId { get; set; }

        public string ModuleName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public bool IsActive { get; set; }
    }
}
