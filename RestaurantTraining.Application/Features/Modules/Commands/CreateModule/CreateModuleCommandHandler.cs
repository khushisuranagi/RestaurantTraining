using MediatR;
using RestaurantTraining.Application.Common.Interfaces;
using RestaurantTraining.Domain.Entities;

namespace RestaurantTraining.Application.Features.Modules.Commands.CreateModule
{
    public class CreateModuleCommandHandler
        : IRequestHandler<CreateModuleCommand, CreateModuleResponse>
    {
        private readonly IModuleRepository _moduleRepository;

        public CreateModuleCommandHandler(IModuleRepository moduleRepository)
        {
            _moduleRepository = moduleRepository;
        }

        public async Task<CreateModuleResponse> Handle(
            CreateModuleCommand request,
            CancellationToken cancellationToken)
        {
            // Simple check so we don't save empty modules.
            if (string.IsNullOrWhiteSpace(request.ModuleName))
            {
                return new CreateModuleResponse
                {
                    Success = false,
                    Message = "Module name is required."
                };
            }

            // Build the new module.
            var module = new Module
            {
                ModuleName = request.ModuleName,
                Description = request.Description,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            // Save it and get the new id back.
            var newModuleId = await _moduleRepository.AddModuleAsync(
                module,
                cancellationToken);

            return new CreateModuleResponse
            {
                Success = true,
                Message = "Module created successfully.",
                ModuleId = newModuleId
            };
        }
    }
}
