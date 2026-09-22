namespace RestaurantTraining.Application.Common.Interfaces
{
    // The PORT: "given a lesson resource's context, produce one MCQ."
    // The Application layer depends only on this — it knows nothing about
    // Gemini, HttpClient, or JSON. Infrastructure provides the adapter.
    public interface IAiQuestionGenerator
    {
        Task<GeneratedMcq?> GenerateAsync(
            QuestionGenerationRequest request, CancellationToken cancellationToken);
    }

    // The text context we hand the generator (never the raw Base64 file data).
    public class QuestionGenerationRequest
    {
        public string LessonTitle { get; set; } = string.Empty;
        public string LessonDescription { get; set; } = string.Empty;
        public string ResourceType { get; set; } = string.Empty;   // "Video", "Article", ...
        public string ResourceUrl { get; set; } = string.Empty;
        public string ContentText { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
    }

    // What the generator returns (a plain MCQ shape, no persistence concerns).
    public class GeneratedMcq
    {
        public string Question { get; set; } = string.Empty;
        public string Explanation { get; set; } = string.Empty;
        public List<GeneratedOption> Options { get; set; } = [];
    }

    public class GeneratedOption
    {
        public string Text { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
    }
}
