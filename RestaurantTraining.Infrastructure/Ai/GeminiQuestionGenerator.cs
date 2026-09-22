using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using RestaurantTraining.Application.Common.Interfaces;

namespace RestaurantTraining.Infrastructure.Ai
{
    // The ADAPTER: the only class that knows Gemini exists.
    // It builds a prompt from the resource context, calls Gemini's
    // generateContent endpoint asking for structured JSON, and parses
    // the reply into a GeneratedMcq. On any failure it returns null.
    public class GeminiQuestionGenerator : IAiQuestionGenerator
    {
        private readonly HttpClient _http;
        private readonly GeminiSettings _settings;

        private static readonly JsonSerializerOptions JsonOptions =
            new() { PropertyNameCaseInsensitive = true };

        public GeminiQuestionGenerator(HttpClient http, IOptions<GeminiSettings> settings)
        {
            _http = http;
            _settings = settings.Value;
        }

        public async Task<GeneratedMcq?> GenerateAsync(
            QuestionGenerationRequest request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(_settings.ApiKey))
                return null; // no key configured yet

            var url =
                $"https://generativelanguage.googleapis.com/v1beta/models/{_settings.Model}:generateContent?key={_settings.ApiKey}";

            var prompt = BuildPrompt(request);

            // Ask Gemini to reply as JSON matching our MCQ schema.
            var body = new
            {
                contents = new[]
                {
                    new { parts = new[] { new { text = prompt } } }
                },
                generationConfig = new
                {
                    responseMimeType = "application/json",
                    responseSchema = new
                    {
                        type = "object",
                        properties = new
                        {
                            question = new { type = "string" },
                            explanation = new { type = "string" },
                            options = new
                            {
                                type = "array",
                                items = new
                                {
                                    type = "object",
                                    properties = new
                                    {
                                        text = new { type = "string" },
                                        isCorrect = new { type = "boolean" }
                                    },
                                    required = new[] { "text", "isCorrect" }
                                }
                            }
                        },
                        required = new[] { "question", "explanation", "options" }
                    }
                }
            };

            try
            {
                HttpResponseMessage? response = null;

                // The free tier occasionally returns "busy" (429/500/503).
                // Retry those a couple of times before giving up.
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

                    return null; // non-transient error
                }

                if (response is null || !response.IsSuccessStatusCode)
                    return null;

                var gemini = await response.Content
                    .ReadFromJsonAsync<GeminiResponse>(JsonOptions, cancellationToken);

                // Newer "thinking" models can return several parts; pick the
                // first one that actually has text (the JSON payload).
                var json = gemini?.Candidates?
                    .FirstOrDefault()?.Content?.Parts?
                    .FirstOrDefault(p => !string.IsNullOrWhiteSpace(p.Text))?.Text;

                if (string.IsNullOrWhiteSpace(json))
                    return null;

                var mcq = JsonSerializer.Deserialize<GeneratedMcq>(json, JsonOptions);

                // Basic sanity checks: need options and exactly one correct answer.
                if (mcq is null
                    || string.IsNullOrWhiteSpace(mcq.Question)
                    || mcq.Options.Count < 2
                    || mcq.Options.Count(o => o.IsCorrect) != 1)
                {
                    return null;
                }

                return mcq;
            }
            catch
            {
                return null;
            }
        }

        private static string BuildPrompt(QuestionGenerationRequest r)
        {
            var sb = new StringBuilder();
            sb.AppendLine("You are creating a training quiz for restaurant staff.");
            sb.AppendLine("Based ONLY on the lesson material below, write exactly ONE");
            sb.AppendLine("multiple-choice question with exactly FOUR options, where exactly");
            sb.AppendLine("one option is correct. Keep it clear and beginner-friendly, and");
            sb.AppendLine("include a short explanation of the correct answer.");
            sb.AppendLine();
            sb.AppendLine($"Lesson title: {r.LessonTitle}");
            if (!string.IsNullOrWhiteSpace(r.LessonDescription))
                sb.AppendLine($"Lesson description: {r.LessonDescription}");
            sb.AppendLine($"Resource type: {r.ResourceType}");
            if (!string.IsNullOrWhiteSpace(r.ResourceUrl))
                sb.AppendLine($"Resource link: {r.ResourceUrl}");
            if (!string.IsNullOrWhiteSpace(r.FileName))
                sb.AppendLine($"File: {r.FileName}");
            if (!string.IsNullOrWhiteSpace(r.ContentText))
                sb.AppendLine($"Notes: {r.ContentText}");
            return sb.ToString();
        }

        // ---- Minimal shapes for parsing Gemini's response envelope ----
        private class GeminiResponse
        {
            public List<Candidate>? Candidates { get; set; }
        }

        private class Candidate
        {
            public Content? Content { get; set; }
        }

        private class Content
        {
            public List<Part>? Parts { get; set; }
        }

        private class Part
        {
            public string? Text { get; set; }
        }
    }
}
