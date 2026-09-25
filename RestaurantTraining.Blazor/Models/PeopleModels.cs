namespace RestaurantTraining.Web.Models;

public class PeopleRoleGroup
{
    public int RoleId { get; set; }
    public string RoleName { get; set; } = string.Empty;
    public List<PeopleModule> Modules { get; set; } = [];
    public List<PeopleUser> Users { get; set; } = [];
}

public class PeopleModule
{
    public int ModuleId { get; set; }
    public string ModuleName { get; set; } = string.Empty;
}

public class PeopleUser
{
    public int UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

public class LearnerProfileModel
{
    public int UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string RoleName { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}

public class SetLearnerActiveStatusRequest
{
    public bool IsActive { get; set; }
}

public class SetLearnerActiveStatusResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}