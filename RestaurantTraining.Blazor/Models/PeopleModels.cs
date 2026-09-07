namespace RestaurantTraining.Blazor.Models;

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
