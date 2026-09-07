using MediatR;
using RestaurantTraining.Application.Common.Interfaces;
using RestaurantTraining.Application.Common.Responses;

namespace RestaurantTraining.Application.Features.AIScenarios.Commands.DeleteAIScenario
{
    public class DeleteAIScenarioCommandHandler
        : IRequestHandler<DeleteAIScenarioCommand, BaseResponse>
    {
        private readonly IAIScenarioRepository _aiScenarioRepository;

        public DeleteAIScenarioCommandHandler(
            IAIScenarioRepository aiScenarioRepository)
        {
            _aiScenarioRepository = aiScenarioRepository;
        }

        public async Task<BaseResponse> Handle(
            DeleteAIScenarioCommand request,
            CancellationToken cancellationToken)
        {
            var scenario =
                await _aiScenarioRepository.GetAIScenarioByIdAsync(
                    request.ScenarioId,
                    cancellationToken);

            if (scenario == null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "AI scenario not found."
                };
            }

            await _aiScenarioRepository.DeleteAIScenarioAsync(
                scenario,
                cancellationToken);

            return new BaseResponse
            {
                Success = true,
                Message = "AI scenario deleted successfully."
            };
        }
    }
}