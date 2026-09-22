namespace RestaurantTraining.Blazor.Models;

public class AssistantChatMessage
{
    public bool FromUser { get; set; }
    public string Text { get; set; } = string.Empty;
}

public class AssistantChatRequest
{
    public List<AssistantChatMessage> Messages { get; set; } = [];
}

public class AssistantChatResult
{
    public string Reply { get; set; } = string.Empty;
}
