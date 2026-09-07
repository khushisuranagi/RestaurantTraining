namespace RestaurantTraining.Application.Common.Authentication
{
    public interface IJwtTokenService
    {
        string GenerateToken(
            int userId,
            string email,
            string fullName,
            string role);
    }
}
