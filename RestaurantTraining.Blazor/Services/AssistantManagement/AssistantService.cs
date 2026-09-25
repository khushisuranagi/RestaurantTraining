using System.Net.Http.Json;
using RestaurantTraining.Web.Models;

namespace RestaurantTraining.Web.Services.AssistantManagement;

public class AssistantService : IAssistantService
{
    private readonly HttpClient _http;

    public AssistantService(HttpClient http)
    {
        _http = http;
    }

    public async Task<string?> ChatAsync(List<AssistantChatMessage> messages)
    {
        var response = await _http.PostAsJsonAsync(
            "/api/assistant/chat",
            new AssistantChatRequest { Messages = messages });

        if (!response.IsSuccessStatusCode)
            return null;

        var result = await response.Content.ReadFromJsonAsync<AssistantChatResult>();
        return result?.Reply;
    }
}
