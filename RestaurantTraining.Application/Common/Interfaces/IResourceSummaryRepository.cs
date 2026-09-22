namespace RestaurantTraining.Application.Common.Interfaces
{
    // Data access for AI resource summaries.
    public interface IResourceSummaryRepository
    {
        // The stored summary text for a resource, or null if not generated yet.
        Task<string?> GetSummaryAsync(int resourceId, CancellationToken cancellationToken);

        // Save a generated summary (one per resource).
        Task SaveSummaryAsync(int resourceId, string summaryText, CancellationToken cancellationToken);

        // The resource's full context INCLUDING the file data, so the model can
        // read the real content. Null if the resource doesn't exist / isn't active.
        Task<ResourceMediaContext?> GetMediaContextAsync(int resourceId, CancellationToken cancellationToken);
    }

    public class ResourceMediaContext
    {
        public string LessonTitle { get; set; } = string.Empty;
        public string LessonDescription { get; set; } = string.Empty;
        public string ResourceType { get; set; } = string.Empty;
        public string ResourceUrl { get; set; } = string.Empty;
        public string ContentText { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string? FileData { get; set; }
        public string? ContentType { get; set; }
    }
}
