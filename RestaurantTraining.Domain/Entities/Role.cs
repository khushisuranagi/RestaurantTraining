namespace RestaurantTraining.Domain.Entities
{
    public class Role
    {
        public int RoleId { get; set; }

        public string RoleName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public ICollection<User> Users { get; set; } = new List<User>();  //a list to store the multiple users of one role
    }
}