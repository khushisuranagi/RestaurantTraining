namespace RestaurantTraining.Application.Common.Interfaces
{
   
    public interface ILearnerModuleRepository
    {
        Task<List<AssignedModuleInfo>> GetAssignedModulesAsync(
            string roleName, CancellationToken cancellationToken);

       
        Task<ModuleContentInfo?> GetAssignedModuleAsync(
            string roleName, int userId, int moduleId, CancellationToken cancellationToken);

      
        Task<bool> CompleteLessonAsync(
            int userId, int lessonId, CancellationToken cancellationToken);
    }

    public class AssignedModuleInfo
    {
        public int ModuleId { get; set; }
        public string ModuleName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    public class ModuleContentInfo
    {
        public int ModuleId { get; set; }
        public string ModuleName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<ModuleLessonInfo> Lessons { get; set; } = [];
    }

    public class ModuleLessonInfo
    {
        public int LessonId { get; set; }
        public string LessonTitle { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int SortOrder { get; set; }
        public bool IsCompleted { get; set; }
        public List<ModuleResourceInfo> Resources { get; set; } = [];
    }

    public class ModuleResourceInfo
    {
        public int ResourceId { get; set; }
        public int ResourceType { get; set; }
        public string ResourceUrl { get; set; } = string.Empty;
        public string? FileData { get; set; }
        public string? FileName { get; set; }
        public string? ContentType { get; set; }
        public string ContentText { get; set; } = string.Empty;
        public int SortOrder { get; set; }
    }
}
