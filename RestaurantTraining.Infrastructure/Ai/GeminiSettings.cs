namespace RestaurantTraining.Infrastructure.Ai
{
    // Bound from the "Gemini" section of configuration.
    public class GeminiSettings
    {
        public string ApiKey { get; set; } = string.Empty;
        public string Model { get; set; } = "gemini-3.6-flash";
    }
}
