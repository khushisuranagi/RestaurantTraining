using MediatR;
using RestaurantTraining.Application.Common.Interfaces;

namespace RestaurantTraining.Application.Features.RoleModules.Queries.GetModuleAssignments
{
    public class GetModuleAssignmentsQueryHandler
        : IRequestHandler<GetModuleAssignmentsQuery, List<ModuleAssignmentDto>>
    {
        private readonly IRoleModuleRepository _roleModuleRepository;

        public GetModuleAssignmentsQueryHandler(IRoleModuleRepository roleModuleRepository)
        {
            _roleModuleRepository = roleModuleRepository;
        }

        public async Task<List<ModuleAssignmentDto>> Handle(
            GetModuleAssignmentsQuery request,
            CancellationToken cancellationToken)
        {
            return await _roleModuleRepository.GetAssignmentsByModuleAsync(
                request.ModuleId, cancellationToken);
        }
    }
}