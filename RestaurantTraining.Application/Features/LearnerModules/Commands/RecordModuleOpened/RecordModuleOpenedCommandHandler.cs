using MediatR;
using RestaurantTraining.Application.Common.Interfaces;

namespace RestaurantTraining.Application.Features.LearnerModules.Commands.RecordModuleOpened
{
    public class RecordModuleOpenedCommandHandler : IRequestHandler<RecordModuleOpenedCommand>
    {
        private readonly ILearnerModuleRepository _repository;

        public RecordModuleOpenedCommandHandler(ILearnerModuleRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(RecordModuleOpenedCommand request, CancellationToken cancellationToken)
        {
            await _repository.RecordModuleOpenedAsync(
                request.UserId, request.ModuleId, cancellationToken);
        }
    }
}