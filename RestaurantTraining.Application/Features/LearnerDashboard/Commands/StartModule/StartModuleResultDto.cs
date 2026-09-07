namespace RestaurantTraining.Application.Features.LearnerDashboard.Commands.StartModule
{
    public class StartModuleResultDto
    {
        // True when the module is not assigned to the learner's role
        // (the controller turns this into a 404 Not Found).
        public bool NotAssigned { get; set; }

        public int ModuleId { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? LastAccessedAt { get; set; }
        public bool IsCompleted { get; set; }
    }
}
