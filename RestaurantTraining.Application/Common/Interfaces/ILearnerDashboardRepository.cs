namespace RestaurantTraining.Application.Common.Interfaces
{
    // Data access for the learner dashboard / "my modules" pages.
   
    public interface ILearnerDashboardRepository
    {
        // Every module assigned to this learner's role
      
        Task<List<LearnerModuleStateInfo>> GetLearnerModuleStatesAsync(
            int userId, CancellationToken cancellationToken);

      
        Task<int> GetDistinctQuizModuleCountAsync(
            List<int> moduleIds, CancellationToken cancellationToken);

        
        Task<int> GetAttemptedQuizModuleCountAsync(
            int userId, List<int> moduleIds, CancellationToken cancellationToken);

        
        Task<List<int>> GetCertifiedModuleIdsAsync(
            int userId, CancellationToken cancellationToken);

        // AI practice scenarios this learner has passed.
        Task<int> GetPassedScenarioCountAsync(
            int userId, CancellationToken cancellationToken);

       
        Task<bool> IsModuleAssignedToLearnerAsync(
            int userId, int moduleId, CancellationToken cancellationToken);

        // Create or touch the learner's progress row for the module.
        Task<StartModuleResultInfo> StartModuleAsync(
            int userId, int moduleId, CancellationToken cancellationToken);
    }

 
    public class LearnerModuleStateInfo
    {
        public int ModuleId { get; set; }
        public string ModuleName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime AssignedAt { get; set; }
        public bool HasStarted { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? LastAccessedAt { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompletedAt { get; set; }
        public int TotalLessons { get; set; }
        public int CompletedLessons { get; set; }
        public bool QuizPassed { get; set; }
    }

    
    public class StartModuleResultInfo
    {
        public int ModuleId { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? LastAccessedAt { get; set; }
        public bool IsCompleted { get; set; }
    }
}
