namespace RestaurantTraining.Web.Services
{
    // holds the details of the user who is currently logged in, in memory so if refreshed data lost
    
    public class AuthState
    {
        public string Token { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;
        public bool IsFirstLogin { get; set; }

        // True if logged in ( token).
        public bool IsLoggedIn => !string.IsNullOrEmpty(Token);
    }
}
