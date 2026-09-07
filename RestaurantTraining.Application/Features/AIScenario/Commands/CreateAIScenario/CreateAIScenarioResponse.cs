using RestaurantTraining.Application.Common.Responses;

namespace RestaurantTraining.Application.Features.AIScenarios.Commands.CreateAIScenario
{
    public class CreateAIScenarioResponse : BaseResponse
    {
        public int ScenarioId { get; set; }
    }
}