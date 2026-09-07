using MediatR;

// requestto create a new Module."
namespace RestaurantTraining.Application.Features.Modules.Commands.CreateModule
{
    public class CreateModuleCommand : IRequest<CreateModuleResponse>
    {
        public string ModuleName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
    }
}
