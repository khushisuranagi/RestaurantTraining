using MediatR;

// A request/message saying: "Give me the list of all modules."
namespace RestaurantTraining.Application.Features.Modules.Queries.GetModules
{
    public class GetModulesQuery : IRequest<List<ModuleDto>>
    {
    }
}
