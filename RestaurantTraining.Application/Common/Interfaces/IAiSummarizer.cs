namespace RestaurantTraining.Application.Common.Interfaces
{
    // The PORT: "summarize this resource's actual content." Infrastructure
    // decides how (send the video/PDF/image/article to Gemini). Application
    // knows nothing about Gemini or HTTP.
    public interface IAiSummarizer
    {
        Task<string?> SummarizeAsync(
            ResourceSummaryInput input, CancellationToken cancellationToken);
    }

    // Everything the summarizer might need — including the actual file data
    // (base64) so the model can read the real content, not just the text.
    public class ResourceSummaryInput
    {
        public string LessonTitle { get; set; } = string.Empty;
        public string LessonDescription { get; set; } = string.Empty;
        public string ResourceType { get; set; } = string.Empty;   // "Video", "Image", "Article", "Document"
        public string ResourceUrl { get; set; } = string.Empty;
        public string ContentText { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string? FileData { get; set; }        // base64 (image / pdf / text file)
        public string? ContentType { get; set; }     // mime type
    }
}
