using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using RestaurantTraining.Application.Common.Interfaces;

namespace RestaurantTraining.Infrastructure.Ai
{
    // The ADAPTER for the help chatbot: a multi-turn plain-text chat with Gemini.
    public class GeminiAssistant : IAiAssistant
    {
        private readonly HttpClient _http;
        private readonly GeminiSettings _settings;

        public GeminiAssistant(HttpClient http, IOptions<GeminiSettings> settings)
        {
            _http = http;
            _settings = settings.Value;
        }

        public async Task<string?> ReplyAsync(
            string systemContext, List<AssistantMessage> messages, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(_settings.ApiKey))
                return null;

            var url =
                $"https://generativelanguage.googleapis.com/v1beta/models/{_settings.Model}:generateContent?key={_settings.ApiKey}";

            var contents = messages
                .Select(m => new
                {
                    role = m.FromUser ? "user" : "model",
                    parts = new[] { new { text = m.Text } }
                })
                .ToArray();

            var body = new
            {
                systemInstruction = new { parts = new[] { new { text = systemContext } } },
                contents
            };

            try
            {
                HttpResponseMessage? response = null;

                for (var attempt = 1; attempt <= 3; attempt++)
                {
                    response = await _http.PostAsJsonAsync(url, body, cancellationToken);

                    if (response.IsSuccessStatusCode)
                        break;

                    var status = (int)response.StatusCode;
                    if ((status == 429 || status == 500 || status == 503) && attempt < 3)
                    {
                        await Task.Delay(1500, cancellationToken);
                        continue;
                    }

                    return null;
                }

                if (response is null || !response.IsSuccessStatusCode)
                    return null;

                var gemini = await response.Content
                    .ReadFromJsonAsync<AssistantResponse>(cancellationToken);

                return gemini?.Candidates?
                    .FirstOrDefault()?.Content?.Parts?
                    .FirstOrDefault(p => !string.IsNullOrWhiteSpace(p.Text))?.Text;
            }
            catch
            {
                return null;
            }
        }

        private class AssistantResponse { public List<Cand>? Candidates { get; set; } }
        private class Cand { public Cont? Content { get; set; } }
        private class Cont { public List<Prt>? Parts { get; set; } }
        private class Prt { public string? Text { get; set; } }
    }
}
