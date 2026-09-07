using RestaurantTraining.Application.Common.Responses;

namespace RestaurantTraining.Application.Features.Auth.Commands.Login
{
    public class LoginResponse : BaseResponse
    {
        public string Token { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;
    }
}