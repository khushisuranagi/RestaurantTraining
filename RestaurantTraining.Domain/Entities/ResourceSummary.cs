namespace RestaurantTraining.Domain.Entities
{
    // An AI-generated text summary of ONE lesson resource.
    // Generated once (on first request) and reused from the database.
    public class ResourceSummary
    {
        public int ResourceSummaryId { get; set; }

        public int ResourceId { get; set; }

        public string SummaryText { get; set; } = string.Empty;

        public DateTime GeneratedAt { get; set; }
    }
}
