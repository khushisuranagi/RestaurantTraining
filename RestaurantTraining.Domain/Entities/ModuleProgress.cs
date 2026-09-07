
namespace RestaurantTraining.Domain.Entities
{
    public class ModuleProgress
    {
        public int ModuleProgressId { get; set; }

        public int UserId { get; set; }

        public int ModuleId { get; set; }

        public bool IsCompleted { get; set; }

        public DateTime? CompletedAt { get; set; }

        public DateTime StartedAt { get; set; }

        public DateTime LastAccessedAt { get; set; }
    }
}
