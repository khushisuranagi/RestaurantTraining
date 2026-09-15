namespace RestaurantTraining.Blazor.Models;

public class QuizResultModel
{
    public decimal Score { get; set; }
    public decimal TotalMarks { get; set; }
    public decimal Percentage { get; set; }
    public int PassingScore { get; set; }
    public bool Passed { get; set; }
    public int CorrectCount { get; set; }
    public int TotalQuestions { get; set; }
    public bool CertificateIssued { get; set; }
    public string? CertificateNumber { get; set; }
    public string ModuleName { get; set; } = string.Empty;
}