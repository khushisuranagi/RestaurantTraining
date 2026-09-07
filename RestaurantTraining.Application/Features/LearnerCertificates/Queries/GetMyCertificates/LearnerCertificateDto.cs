namespace RestaurantTraining.Application.Features.LearnerCertificates.Queries.GetMyCertificates
{
    // One earned certificate, shaped exactly like the JSON the Blazor
    // "My Certificates" page already expects (field names unchanged).
    public class LearnerCertificateDto
    {
        public int CertificateId { get; set; }
        public string CertificateNumber { get; set; } = string.Empty;
        public string ModuleName { get; set; } = string.Empty;
        public DateTime IssuedDate { get; set; }
        public string LearnerName { get; set; } = string.Empty;

        // Best passing score for that module (percentage). Null if unknown.
        public decimal? ScorePercent { get; set; }
    }
}
