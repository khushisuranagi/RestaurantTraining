namespace RestaurantTraining.Blazor.Models
{
    //  matches the json that the api sends back from POST /api/Auth/login.
    //  API's LoginResponse.
    public class LoginResult
    {
        public bool Success { get; set; }

        public string Message { get; set; } = string.Empty;

        public string Token { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;
    }
}
