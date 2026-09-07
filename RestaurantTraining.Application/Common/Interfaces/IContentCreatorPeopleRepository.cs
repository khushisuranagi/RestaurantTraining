namespace RestaurantTraining.Application.Common.Interfaces
{
    // Data access for the Content Creator "People" page.
   
    public interface IContentCreatorPeopleRepository
    {
        Task<List<PeopleRoleInfo>> GetRolesAsync(CancellationToken cancellationToken);

        Task<List<PeopleUserInfo>> GetUsersAsync(CancellationToken cancellationToken);

        // Each role paired with a module assigned to it.
        Task<List<PeopleRoleModuleInfo>> GetRoleModulesAsync(CancellationToken cancellationToken);
    }

    public class PeopleRoleInfo
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;
    }

    public class PeopleUserInfo
    {
        public int UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int RoleId { get; set; }
    }

    public class PeopleRoleModuleInfo
    {
        public int RoleId { get; set; }
        public int ModuleId { get; set; }
        public string ModuleName { get; set; } = string.Empty;
    }
}
