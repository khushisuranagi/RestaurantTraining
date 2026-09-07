using MediatR;
using RestaurantTraining.Application.Common.Interfaces;

namespace RestaurantTraining.Application.Features.LearnerModules.Queries.GetAssignedModules
{
    public class GetAssignedModulesQueryHandler
        : IRequestHandler<GetAssignedModulesQuery, List<AssignedModuleDto>>
    {
        private readonly ILearnerModuleRepository _repository;

        public GetAssignedModulesQueryHandler(
            ILearnerModuleRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<AssignedModuleDto>> Handle(
            GetAssignedModulesQuery request,
            CancellationToken cancellationToken)
        {
            var modules = await _repository.GetAssignedModulesAsync(
                request.RoleName, cancellationToken);

            return modules
                .Select(m => new AssignedModuleDto
                {
                    ModuleId = m.ModuleId,
                    ModuleName = m.ModuleName,
                    Description = m.Description,
                    CreatedAt = m.CreatedAt
                })
                .ToList();
        }
    }
}
