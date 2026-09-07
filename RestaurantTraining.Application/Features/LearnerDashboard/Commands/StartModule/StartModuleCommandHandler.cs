using MediatR;
using RestaurantTraining.Application.Common.Interfaces;

namespace RestaurantTraining.Application.Features.LearnerDashboard.Commands.StartModule
{
    public class StartModuleCommandHandler
        : IRequestHandler<StartModuleCommand, StartModuleResultDto>
    {
        private readonly ILearnerDashboardRepository _repository;

        public StartModuleCommandHandler(
            ILearnerDashboardRepository repository)
        {
            _repository = repository;
        }

        public async Task<StartModuleResultDto> Handle(
            StartModuleCommand request,
            CancellationToken cancellationToken)
        {
            var isAssigned = await _repository.IsModuleAssignedToLearnerAsync(
                request.UserId, request.ModuleId, cancellationToken);

            if (!isAssigned)
            {
                return new StartModuleResultDto { NotAssigned = true };
            }

            var result = await _repository.StartModuleAsync(
                request.UserId, request.ModuleId, cancellationToken);

            return new StartModuleResultDto
            {
                NotAssigned = false,
                ModuleId = result.ModuleId,
                StartedAt = result.StartedAt,
                LastAccessedAt = result.LastAccessedAt,
                IsCompleted = result.IsCompleted
            };
        }
    }
}
