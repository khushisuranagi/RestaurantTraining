using MediatR;

namespace RestaurantTraining.Application.Features.Assistant.Commands.ChatAssistant
{
    public class ChatAssistantCommand : IRequest<ChatAssistantResult>
    {
        public int UserId { get; set; }
        // Set by the controller from the JWT (to tailor the answer).
        public string RoleName { get; set; } = string.Empty;

        // The whole conversation so far (the user's new message last).
        public List<AssistantMessageInput> Messages { get; set; } = [];
    }

    public class AssistantMessageInput
    {
        public bool FromUser { get; set; }
        public string Text { get; set; } = string.Empty;
    }

    public class ChatAssistantResult
    {
        public string Reply { get; set; } = string.Empty;
    }
}
