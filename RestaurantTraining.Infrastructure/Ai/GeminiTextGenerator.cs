using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using RestaurantTraining.Application.Common.Interfaces;

namespace RestaurantTraining.Infrastructure.Ai
{
    // Plain text generation against Gemini's generateContent endpoint.
    // Same pattern as GeminiQuestionGenerator, but returns raw text.
    public class GeminiTextGenerator : IAiTextGenerator
    {
        private readonly HttpClient _http;
        private readonly GeminiSettings _settings;

        public GeminiTextGenerator(HttpClient http, IOptions<GeminiSettings> settings)
        {
            _http = http;
            _settings = settings.Value;
        }

        public async Task<string?> GenerateTextAsync(string prompt, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(_settings.ApiKey))
                return null; // no key configured

            var url =
                $"https://generativelanguage.googleapis.com/v1beta/models/{_settings.Model}:generateContent?key={_settings.ApiKey}";

            var body = new
            {
                contents = new[]
                {
                    new { parts = new[] { new { text = prompt } } }
                }
            };

            try
            {
                HttpResponseMessage? response = null;

                // Retry the transient "busy" responses a couple of times.
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
                    .ReadFromJsonAsync<GeminiTextResponse>(cancellationToken);

                return gemini?.Candidates?
                    .FirstOrDefault()?.Content?.Parts?
                    .FirstOrDefault(p => !string.IsNullOrWhiteSpace(p.Text))?.Text;
            }
            catch
            {
                return null;
            }
        }

        // Minimal shapes for parsing the response envelope.
        private class GeminiTextResponse
        {
            public List<TextCandidate>? Candidates { get; set; }
        }

        private class TextCandidate
        {
            public TextContent? Content { get; set; }
        }

        private class TextContent
        {
            public List<TextPart>? Parts { get; set; }
        }

        private class TextPart
        {
            public string? Text { get; set; }
        }
    }
}
