using MediatR;

namespace RestaurantTraining.Application.Features.ModuleCertificateSettings.Queries.GetModuleCertificateSetting
{
    // Returns null when the module has no certificate configured yet.
    public class GetModuleCertificateSettingQuery
        : IRequest<ModuleCertificateSettingDto?>
    {
        public int ModuleId { get; set; }
    }
}