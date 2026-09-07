using MediatR;
using RestaurantTraining.Application.Common.Interfaces;

namespace RestaurantTraining.Application.Features.Modules.Queries.GetModules
{
    public class GetModulesQueryHandler
        : IRequestHandler<GetModulesQuery, List<ModuleDto>>
    {
        private readonly IModuleRepository _moduleRepository;

        public GetModulesQueryHandler(IModuleRepository moduleRepository)
        {
            _moduleRepository = moduleRepository;
        }

        public async Task<List<ModuleDto>> Handle(
            GetModulesQuery request,
            CancellationToken cancellationToken)
        {
            var modules = await _moduleRepository.GetAllModulesAsync(
                cancellationToken);

            // Convert each database Module into a simple ModuleDto.
            var result = new List<ModuleDto>();

            foreach (var module in modules)
            {
                result.Add(new ModuleDto
                {
                    ModuleId = module.ModuleId,
                    ModuleName = module.ModuleName,
                    Description = module.Description,
                    IsActive = module.IsActive,
                    CreatedAt = module.CreatedAt
                });
            }

            return result;
        }
    }
}
