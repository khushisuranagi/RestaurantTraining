using MediatR;
using RestaurantTraining.Application.Common.Responses;

namespace RestaurantTraining.Application.Features.AIScenarios.Commands.DeleteAIScenario
{
    public class DeleteAIScenarioCommand : IRequest<BaseResponse>
    {
        public int ScenarioId { get; set; }
    }
}