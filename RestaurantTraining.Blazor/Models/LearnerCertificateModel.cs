namespace RestaurantTraining.Web.Models;

public class LearnerCertificateModel
{
    public int CertificateId { get; set; }
    public string CertificateNumber { get; set; } = string.Empty;
    public string ModuleName { get; set; } = string.Empty;
    public string LearnerName { get; set; } = string.Empty;
    public DateTime IssuedDate { get; set; }
    public decimal? ScorePercent { get; set; }
}