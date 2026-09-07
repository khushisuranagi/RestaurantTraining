using MediatR;

namespace RestaurantTraining.Application.Features.AIScenarios.Queries.GetAIScenarios
{
    public class GetAIScenariosQuery : IRequest<List<AIScenarioDto>>
    {
    }
}