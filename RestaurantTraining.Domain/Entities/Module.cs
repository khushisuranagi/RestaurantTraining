namespace RestaurantTraining.Domain.Entities
{
    public class Module
    {
        public int ModuleId { get; set; }
        public string ModuleName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        
        public DateTime CreatedAt { get; set; }
    }
}