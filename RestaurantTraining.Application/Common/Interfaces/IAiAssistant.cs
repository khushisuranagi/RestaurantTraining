namespace RestaurantTraining.Application.Common.Interfaces
{
    // The PORT for the help chatbot. Infrastructure (Gemini) provides the adapter.
    public interface IAiAssistant
    {
        Task<string?> ReplyAsync(
            string systemContext, List<AssistantMessage> messages, CancellationToken cancellationToken);
    }

    public class AssistantMessage
    {
        public bool FromUser { get; set; }   // true = the person, false = the assistant
        public string Text { get; set; } = string.Empty;
    }
}
