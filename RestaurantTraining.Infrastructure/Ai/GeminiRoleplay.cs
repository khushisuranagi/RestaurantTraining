using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Options;
using RestaurantTraining.Application.Common.Interfaces;

namespace RestaurantTraining.Infrastructure.Ai
{
    // The ADAPTER for the AI practice scenario
    //  - generates a role based scenario 
    //  - role-plays the customer + grades each turn 
    public class GeminiRoleplay : IAiRoleplay
    {
        private readonly HttpClient _http;
        private readonly GeminiSettings _settings;

        private static readonly JsonSerializerOptions JsonOptions =
            new() { PropertyNameCaseInsensitive = true };

        public GeminiRoleplay(HttpClient http, IOptions<GeminiSettings> settings)
        {
            _http = http;
            _settings = settings.Value;
        }

        public async Task<GeneratedScenario?> GenerateScenarioAsync(
            string moduleTitle, string roleName, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(_settings.ApiKey))
                return null;

            var prompt =
                $"Create ONE realistic customer-service practice scenario to train a restaurant '{roleName}'. " +
                $"The trainee just completed the training module '{moduleTitle}'. " +
                $"Invent a situation that genuinely fits a {roleName}'s job (do NOT use situations outside their duties) " +
                "where a customer has a problem the trainee must handle in a live text conversation. " +
                "Return JSON with: " +
                "title (a few words); " +
                "description (2–3 sentences describing the situation to the trainee, making clear the customer's problem and that the trainee must respond as the " + roleName + "); " +
                "scenarioPrompt (a system instruction for an AI to role-play THAT specific customer in a text chat — stay in character, react naturally, and be satisfied ONLY when the " + roleName + " genuinely resolves the issue well); " +
                "category (e.g. 'Complaint').";

            var body = new
            {
                contents = new[] { new { parts = new[] { new { text = prompt } } } },
                generationConfig = new
                {
                    responseMimeType = "application/json",
                    responseSchema = new
                    {
                        type = "object",
                        properties = new
                        {
                            title = new { type = "string" },
                            description = new { type = "string" },
                            scenarioPrompt = new { type = "string" },
                            category = new { type = "string" }
                        },
                        required = new[] { "title", "description", "scenarioPrompt", "category" }
                    }
                }
            };

            var json = await CallAsync(body, cancellationToken);
            if (string.IsNullOrWhiteSpace(json))
                return null;

            try
            {
                var scenario = JsonSerializer.Deserialize<GeneratedScenario>(json, JsonOptions);
                if (scenario is null
                    || string.IsNullOrWhiteSpace(scenario.Title)
                    || string.IsNullOrWhiteSpace(scenario.Description)
                    || string.IsNullOrWhiteSpace(scenario.ScenarioPrompt))
                {
                    return null;
                }
                return scenario;
            }
            catch
            {
                return null;
            }
        }

        public async Task<RoleplayTurn?> ContinueAsync(
            RoleplayContext context, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(_settings.ApiKey))
                return null;

            var systemText =
                context.ScenarioPrompt + "\n\n" +
                "You are role-playing this customer in a text conversation with a restaurant staff member. " +
                "Reply naturally and briefly, staying fully in character. Decide whether you are now satisfied " +
                "that the staff member has genuinely and correctly resolved your issue (be reasonably strict — " +
                "a vague or unhelpful reply should NOT satisfy you). Respond ONLY as JSON with: " +
                "reply (your next message to the staff member), " +
                "satisfied (true only when the issue is properly handled), " +
                "feedback (one short coaching sentence for the trainee).";

            // Map the conversation: learner → "user", customer → "model".
            var contents = context.Messages
                .Select(m => new
                {
                    role = m.FromLearner ? "user" : "model",
                    parts = new[] { new { text = m.Text } }
                })
                .ToArray();

            var body = new
            {
                systemInstruction = new { parts = new[] { new { text = systemText } } },
                contents,
                generationConfig = new
                {
                    responseMimeType = "application/json",
                    responseSchema = new
                    {
                        type = "object",
                        properties = new
                        {
                            reply = new { type = "string" },
                            satisfied = new { type = "boolean" },
                            feedback = new { type = "string" }
                        },
                        required = new[] { "reply", "satisfied", "feedback" }
                    }
                }
            };

            var json = await CallAsync(body, cancellationToken);
            if (string.IsNullOrWhiteSpace(json))
                return null;

            try
            {
                var turn = JsonSerializer.Deserialize<RoleplayTurn>(json, JsonOptions);
                return string.IsNullOrWhiteSpace(turn?.Reply) ? null : turn;
            }
            catch
            {
                return null;
            }
        }

        // Posts the request (with transient-retry) and returns the JSON text part.
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
                    .ReadFromJsonAsync<RoleplayResponse>(JsonOptions, cancellationToken);

                return gemini?.Candidates?
                    .FirstOrDefault()?.Content?.Parts?
                    .FirstOrDefault(p => !string.IsNullOrWhiteSpace(p.Text))?.Text;
            }
            catch
            {
                return null;
            }
        }

        private class RoleplayResponse { public List<Cand>? Candidates { get; set; } }
        private class Cand { public Cont? Content { get; set; } }
        private class Cont { public List<Prt>? Parts { get; set; } }
        private class Prt { public string? Text { get; set; } }
    }
}
