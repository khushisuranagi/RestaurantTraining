namespace RestaurantTraining.Application.Features.ContentCreatorPeople.Queries.GetPeople
{
    // One role, with the people in it and the modules assigned to it.
    // Field names match the Blazor "People" page models so JSON is unchanged.
    public class PeopleRoleGroupDto
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public List<PeopleModuleDto> Modules { get; set; } = [];
        public List<PeopleUserDto> Users { get; set; } = [];
    }

    public class PeopleModuleDto
    {
        public int ModuleId { get; set; }
        public string ModuleName { get; set; } = string.Empty;
    }

    public class PeopleUserDto
    {
        public int UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
