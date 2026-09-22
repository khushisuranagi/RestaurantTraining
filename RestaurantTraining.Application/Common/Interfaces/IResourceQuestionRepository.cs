namespace RestaurantTraining.Application.Common.Interfaces
{
    // Data access for AI-generated resource questions.
    public interface IResourceQuestionRepository
    {
        // The text context to feed the generator. Null if the resource
        // doesn't exist / isn't active.
        Task<ResourceContextInfo?> GetResourceContextAsync(
            int resourceId, CancellationToken cancellationToken);

        // The stored question for a resource (with correct flags), or null.
        Task<StoredResourceQuestion?> GetByResourceIdAsync(
            int resourceId, CancellationToken cancellationToken);

        // Save a freshly generated question + options, and return it stored
        // (with the new database ids).
        Task<StoredResourceQuestion> SaveGeneratedAsync(
            int resourceId, GeneratedMcq mcq, CancellationToken cancellationToken);

        // Has this learner already answered this resource's MCQ correctly?
        Task<bool> HasAnsweredCorrectlyAsync(
            int userId, int resourceId, CancellationToken cancellationToken);

        // Record a correct answer + award points (only the first time).
        // Returns the points awarded this call (0 if already recorded).
        Task<int> RecordCorrectAnswerAsync(
            int userId, int resourceId, int points, CancellationToken cancellationToken);

        // The learner's total points from resource questions.
        Task<int> GetTotalPointsAsync(int userId, CancellationToken cancellationToken);
    }

    public class ResourceContextInfo
    {
        public string LessonTitle { get; set; } = string.Empty;
        public string LessonDescription { get; set; } = string.Empty;
        public string ResourceType { get; set; } = string.Empty;
        public string ResourceUrl { get; set; } = string.Empty;
        public string ContentText { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
    }

    public class StoredResourceQuestion
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; } = string.Empty;
        public string Explanation { get; set; } = string.Empty;
        public List<StoredResourceOption> Options { get; set; } = [];
    }

    public class StoredResourceOption
    {
        public int OptionId { get; set; }
        public string OptionText { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
    }
}
