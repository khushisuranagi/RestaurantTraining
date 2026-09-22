using RestaurantTraining.Blazor.Models;

namespace RestaurantTraining.Blazor.Services.AssistantManagement;

public interface IAssistantService
{
    Task<string?> ChatAsync(List<AssistantChatMessage> messages);
}
