using RestaurantTraining.Application.Common.Responses;

namespace RestaurantTraining.Application.Features.ModuleCertificateSettings.Commands.SaveModuleCertificateSetting
{
    public class SaveModuleCertificateSettingResponse : BaseResponse
    {
        public int SettingId { get; set; }
    }
}