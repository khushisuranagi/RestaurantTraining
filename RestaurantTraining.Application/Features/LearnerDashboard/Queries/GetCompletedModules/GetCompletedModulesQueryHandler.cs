using MediatR;
using RestaurantTraining.Application.Common.Interfaces;

namespace RestaurantTraining.Application.Features.LearnerDashboard.Queries.GetCompletedModules
{
    public class GetCompletedModulesQueryHandler
        : IRequestHandler<GetCompletedModulesQuery, List<CompletedModuleDto>>
    {
        private readonly ILearnerDashboardRepository _repository;

        public GetCompletedModulesQueryHandler(
            ILearnerDashboardRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<CompletedModuleDto>> Handle(
            GetCompletedModulesQuery request,
            CancellationToken cancellationToken)
        {
            var modules = await _repository.GetLearnerModuleStatesAsync(
                request.UserId, cancellationToken);

            return modules
                .Where(x => x.IsCompleted)
                .OrderByDescending(x => x.CompletedAt)
                .Select(x => new CompletedModuleDto
                {
                    ModuleId = x.ModuleId,
                    ModuleName = x.ModuleName,
                    Description = x.Description,
                    CompletedAt = x.CompletedAt
                })
                .ToList();
        }
    }
}
