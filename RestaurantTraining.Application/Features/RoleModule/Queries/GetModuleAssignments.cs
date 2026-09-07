namespace RestaurantTraining.Application.Features.RoleModules.Queries.GetModuleAssignments
{
    public class ModuleAssignmentDto
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public DateTime AssignedAt { get; set; }
    }
}