using MediatR;

namespace RestaurantTraining.Application.Features.ModuleCertificateSettings.Commands.SaveModuleCertificateSetting
{
    public class SaveModuleCertificateSettingCommand
        : IRequest<SaveModuleCertificateSettingResponse>
    {
        public int ModuleId { get; set; }
        public bool IsEnabled { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public int MinimumPassingScore { get; set; }
        public string Template { get; set; } = string.Empty;
        public string IssuerName { get; set; } = string.Empty;
    }
}