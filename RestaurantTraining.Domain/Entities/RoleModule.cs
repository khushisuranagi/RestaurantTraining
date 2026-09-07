namespace RestaurantTraining.Domain.Entities
{
    public class RoleModule
    {
        public int RoleModuleId { get; set; }

        public int RoleId { get; set; }

        public int ModuleId { get; set; }

        public DateTime AssignedAt { get; set; }
    }
}