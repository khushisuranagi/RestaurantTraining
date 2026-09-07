using MediatR;
using RestaurantTraining.Application.Common.Interfaces;
using RestaurantTraining.Domain.Entities;

namespace RestaurantTraining.Application.Features.ModuleCertificateSettings.Commands.SaveModuleCertificateSetting
{
    public class SaveModuleCertificateSettingCommandHandler
        : IRequestHandler<SaveModuleCertificateSettingCommand, SaveModuleCertificateSettingResponse>
    {
        private readonly IModuleCertificateSettingRepository _repository;

        public SaveModuleCertificateSettingCommandHandler(
            IModuleCertificateSettingRepository repository)
        {
            _repository = repository;
        }

        public async Task<SaveModuleCertificateSettingResponse> Handle(
            SaveModuleCertificateSettingCommand request,
            CancellationToken cancellationToken)
        {
            // Validate only the things that matter when the certificate is ON.
            if (request.IsEnabled)
            {
                if (string.IsNullOrWhiteSpace(request.Title))
                {
                    return new SaveModuleCertificateSettingResponse
                    {
                        Success = false,
                        Message = "Certificate title is required when the certificate is enabled."
                    };
                }

                if (request.MinimumPassingScore < 0 ||
                    request.MinimumPassingScore > 100)
                {
                    return new SaveModuleCertificateSettingResponse
                    {
                        Success = false,
                        Message = "Minimum passing score must be between 0 and 100."
                    };
                }
            }

            // Upsert: one setting per module.
            var setting = await _repository.GetByModuleIdAsync(
                request.ModuleId,
                cancellationToken);

            if (setting == null)
            {
                setting = new ModuleCertificateSetting
                {
                    ModuleId = request.ModuleId,
                    IsEnabled = request.IsEnabled,
                    Title = request.Title,
                    Message = request.Message,
                    MinimumPassingScore = request.MinimumPassingScore,
                    Template = request.Template,
                    IssuerName = request.IssuerName
                };

                var newId = await _repository.AddAsync(setting, cancellationToken);

                return new SaveModuleCertificateSettingResponse
                {
                    Success = true,
                    Message = "Certificate settings saved.",
                    SettingId = newId
                };
            }

            setting.IsEnabled = request.IsEnabled;
            setting.Title = request.Title;
            setting.Message = request.Message;
            setting.MinimumPassingScore = request.MinimumPassingScore;
            setting.Template = request.Template;
            setting.IssuerName = request.IssuerName;

            await _repository.UpdateAsync(setting, cancellationToken);

            return new SaveModuleCertificateSettingResponse
            {
                Success = true,
                Message = "Certificate settings updated.",
                SettingId = setting.SettingId
            };
        }
    }
}