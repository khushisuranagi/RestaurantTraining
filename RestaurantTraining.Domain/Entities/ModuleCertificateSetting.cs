namespace RestaurantTraining.Domain.Entities
{
    // The Content Creator's certificate configuration for ONE module.
    // (Different from Certificate.cs, which is an issued certificate per learner.)
    public class ModuleCertificateSetting
    {
        public int SettingId { get; set; }

        public int ModuleId { get; set; }          // one setting per module

        public bool IsEnabled { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public int MinimumPassingScore { get; set; }   // percentage, e.g. 70

        public string Template { get; set; } = string.Empty;   // design name

        public string IssuerName { get; set; } = string.Empty;
    }
}