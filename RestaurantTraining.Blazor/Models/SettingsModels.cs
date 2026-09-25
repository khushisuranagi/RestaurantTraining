namespace RestaurantTraining.Web.Models;

public class ChangePasswordRequest
{
    public string CurrentPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
    public string ConfirmNewPassword { get; set; } = string.Empty;
}







public class ChangePasswordResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
}

public class UpdateEmailRequest
{
    public string CurrentPassword { get; set; } = string.Empty;
    public string NewEmail { get; set; } = string.Empty;
}

public class UpdateEmailResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
}

public class DeleteAccountRequest
{
    public string CurrentPassword { get; set; } = string.Empty;
}

public class DeleteAccountResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
}