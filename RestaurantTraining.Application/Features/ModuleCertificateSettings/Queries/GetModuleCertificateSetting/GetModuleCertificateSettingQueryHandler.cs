using MediatR;
using RestaurantTraining.Application.Common.Interfaces;

namespace RestaurantTraining.Application.Features.ModuleCertificateSettings.Queries.GetModuleCertificateSetting
{
    public class GetModuleCertificateSettingQueryHandler
        : IRequestHandler<GetModuleCertificateSettingQuery, ModuleCertificateSettingDto?>
    {
        private readonly IModuleCertificateSettingRepository _repository;

        public GetModuleCertificateSettingQueryHandler(
            IModuleCertificateSettingRepository repository)
        {
            _repository = repository;
        }

        public async Task<ModuleCertificateSettingDto?> Handle(
            GetModuleCertificateSettingQuery request,
            CancellationToken cancellationToken)
        {
            var setting = await _repository.GetByModuleIdAsync(
                request.ModuleId,
                cancellationToken);

            if (setting == null)
                return null;

            return new ModuleCertificateSettingDto
            {
                SettingId = setting.SettingId,
                ModuleId = setting.ModuleId,
                IsEnabled = setting.IsEnabled,
                Title = setting.Title,
                Message = setting.Message,
                MinimumPassingScore = setting.MinimumPassingScore,
                Template = setting.Template,
                IssuerName = setting.IssuerName
            };
        }
    }
}