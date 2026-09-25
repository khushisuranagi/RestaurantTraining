using RestaurantTraining.Web.Models;

namespace RestaurantTraining.Web.Services.AssistantManagement;

public interface IAssistantService
{
    Task<string?> ChatAsync(List<AssistantChatMessage> messages);
}
