using System.Net.Http.Json;
using System.Text;
using Microsoft.Extensions.Options;
using RestaurantTraining.Application.Common.Interfaces;

namespace RestaurantTraining.Infrastructure.Ai
{
    // Summarizes a resource's ACTUAL content by sending it to Gemini:
    //  - Video    → the YouTube URL (video understanding)
    //  - Article  → the URL via the url_context tool
    //  - PDF/Image→ the base64 file as inline_data
    //  - Text file→ the decoded text
    // MEDIA_RESOLUTION_LOW + a short-summary prompt keep token use down, and the
    // result is cached by the caller so a resource is only summarized once.
    // If the rich request fails, it falls back to a text-only summary.
    public class GeminiSummarizer : IAiSummarizer
    {
        private readonly HttpClient _http;
        private readonly GeminiSettings _settings;

        private const string Prompt =
            "You are helping a restaurant trainee. Summarize this training material in " +
            "3–5 bullet points ONLY,the answer should not be in paragraph format, covering the key points so the " +
            "learner understands it without opening the original." +
            "Reply in plain text only, with no Markdown formatting: no asterisks for bold or italics" +
            "no backticks, no headings, no bullet characters like '-' or '*'. Use plain sentences " +
            "and, if you need a list, write it as short numbered sentences instead. ";

        public GeminiSummarizer(HttpClient http, IOptions<GeminiSettings> settings)
        {
            _http = http;
            _settings = settings.Value;
        }

        public async Task<string?> SummarizeAsync(
            ResourceSummaryInput input, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(_settings.ApiKey))
                return null;

            // Try a content-based (multimodal) summary first.
            var richBody = BuildRichBody(input);
            if (richBody is not null)
            {
                var text = await CallAsync(richBody, cancellationToken);
                if (!string.IsNullOrWhiteSpace(text))
                    return text;
            }

            // Fallback: summarize from the text we have.
            return await CallAsync(BuildTextOnlyBody(input), cancellationToken);
        }

        // Builds the best request for the resource type, or null if the type has
        // no rich content we can send (→ caller uses the text-only fallback).
        private static object? BuildRichBody(ResourceSummaryInput r)
        {
            var context = $" Lesson: {r.LessonTitle}. {r.LessonDescription}";
            var type = r.ResourceType?.ToLowerInvariant();
            var lowRes = new { mediaResolution = "MEDIA_RESOLUTION_LOW" };

            // VIDEO (YouTube link)
            if (type == "video" && !string.IsNullOrWhiteSpace(r.ResourceUrl))
            {
                return new
                {
                    contents = new[] { new { parts = new object[]
                    {
                        new { text = Prompt + context + " Summarize this video." },
                        new { file_data = new { file_uri = r.ResourceUrl } }
                    } } },
                    generationConfig = lowRes
                };
            }

            // ARTICLE (fetch the page with the url_context tool)
            if (type == "article" && !string.IsNullOrWhiteSpace(r.ResourceUrl))
            {
                return new
                {
                    contents = new[] { new { parts = new object[]
                    {
                        new { text = Prompt + context + $" Summarize the article at this link: {r.ResourceUrl}" }
                    } } },
                    tools = new[] { new { url_context = new { } } }
                };
            }

            // IMAGE (inline base64)
            if (type == "image"
                && !string.IsNullOrWhiteSpace(r.FileData)
                && !string.IsNullOrWhiteSpace(r.ContentType))
            {
                return new
                {
                    contents = new[] { new { parts = new object[]
                    {
                        new { text = Prompt + context + " Summarize what this image shows." },
                        new { inline_data = new { mime_type = r.ContentType, data = r.FileData } }
                    } } },
                    generationConfig = lowRes
                };
            }

            // DOCUMENT
            if (type == "document")
            {
                // PDF → inline document
                if (r.ContentType == "application/pdf" && !string.IsNullOrWhiteSpace(r.FileData))
                {
                    return new
                    {
                        contents = new[] { new { parts = new object[]
                        {
                            new { text = Prompt + context + " Summarize this document." },
                            new { inline_data = new { mime_type = "application/pdf", data = r.FileData } }
                        } } },
                        generationConfig = lowRes
                    };
                }

                // Plain text file → just send the decoded text (cheapest).
                if (r.ContentType == "text/plain" && !string.IsNullOrWhiteSpace(r.FileData))
                {
                    var decoded = TryDecodeBase64(r.FileData);
                    if (!string.IsNullOrWhiteSpace(decoded))
                    {
                        return new
                        {
                            contents = new[] { new { parts = new[]
                            {
                                new { text = Prompt + context + " Summarize this document:\n\n" + Truncate(decoded, 20000) }
                            } } }
                        };
                    }
                }
                // Other document types (e.g. docx) fall through to text-only.
            }

            return null;
        }

        private static object BuildTextOnlyBody(ResourceSummaryInput r)
        {
            var sb = new StringBuilder();
            sb.Append(Prompt);
            sb.Append($" Lesson: {r.LessonTitle}. {r.LessonDescription}. Resource type: {r.ResourceType}.");
            if (!string.IsNullOrWhiteSpace(r.ContentText))
                sb.Append(" Notes: " + r.ContentText);

            return new
            {
                contents = new[] { new { parts = new[] { new { text = sb.ToString() } } } }
            };
        }

        private async Task<string?> CallAsync(object body, CancellationToken cancellationToken)
        {
            var url =
                $"https://generativelanguage.googleapis.com/v1beta/models/{_settings.Model}:generateContent?key={_settings.ApiKey}";

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
                    .ReadFromJsonAsync<SummaryResponse>(cancellationToken);

                return gemini?.Candidates?
                    .FirstOrDefault()?.Content?.Parts?
                    .FirstOrDefault(p => !string.IsNullOrWhiteSpace(p.Text))?.Text;
            }
            catch
            {
                return null;
            }
        }

        private static string? TryDecodeBase64(string base64)
        {
            try { return Encoding.UTF8.GetString(Convert.FromBase64String(base64)); }
            catch { return null; }
        }

        private static string Truncate(string text, int max) =>
            text.Length <= max ? text : text[..max];

        // Response envelope shapes.
        private class SummaryResponse { public List<Cand>? Candidates { get; set; } }
        private class Cand { public Cont? Content { get; set; } }
        private class Cont { public List<Prt>? Parts { get; set; } }
        private class Prt { public string? Text { get; set; } }
    }
}
