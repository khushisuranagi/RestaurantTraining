namespace RestaurantTraining.Application.Common.Interfaces
{
    // Data access for the signed-in user's own profile.
    public interface IProfileRepository
    {
        // The user's details with the role name joined in. Null if not found.
        Task<ProfileInfo?> GetProfileAsync(int userId, CancellationToken cancellationToken);
    }

    public class ProfileInfo
    {
        public int UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
