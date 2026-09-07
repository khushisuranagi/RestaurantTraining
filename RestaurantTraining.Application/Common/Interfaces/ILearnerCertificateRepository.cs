namespace RestaurantTraining.Application.Common.Interfaces
{
    // Data access for a learner's own certificates.
   
    public interface ILearnerCertificateRepository
    {
      
        Task<string> GetLearnerNameAsync(int userId, CancellationToken cancellationToken);

        // The learner's certificates
        Task<List<LearnerCertificateInfo>> GetCertificatesAsync(
            int userId, CancellationToken cancellationToken);

     
        Task<List<LearnerAttemptInfo>> GetPassedAttemptsAsync(
            int userId, List<int> moduleIds, CancellationToken cancellationToken);
    }

 
    public class LearnerCertificateInfo
    {
        public int CertificateId { get; set; }
        public string CertificateNumber { get; set; } = string.Empty;
        public int ModuleId { get; set; }
        public string ModuleName { get; set; } = string.Empty;
        public DateTime IssuedDate { get; set; }
    }

    public class LearnerAttemptInfo
    {
        public int ModuleId { get; set; }
        public decimal Score { get; set; }
        public decimal TotalMarks { get; set; }
    }
}
