namespace RestaurantTraining.Blazor.Models
{

    public class ProfileModel
    {
        public int UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
    }

    // Sent to the API when the user edits their name / phone.
    public class UpdateProfileRequest
    {
        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }

    // The API's reply to a profile update.
    public class ProfileUpdateResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}

