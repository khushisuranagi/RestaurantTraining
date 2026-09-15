namespace RestaurantTraining.Application.Common.Interfaces
{
    public interface ICertificatePdfGenerator
    {
        byte[] Generate(CertificatePdfData data);
    }

    public class CertificatePdfData
    {
        public string LearnerName { get; set; } = string.Empty;
        public string ModuleName { get; set; } = string.Empty;
        public string CertificateNumber { get; set; } = string.Empty;
        public DateTime IssuedDate { get; set; }
        public decimal? ScorePercent { get; set; }
    }
}