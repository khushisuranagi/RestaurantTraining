namespace RestaurantTraining.Application.Common.Interfaces
{
    // Issues a module certificate once the learner has met BOTH gates
    // (passed the quiz AND the practice scenario).
    public interface ICertificateIssuanceRepository
    {
        Task<bool> HasPassedModuleQuizAsync(
            int userId, int moduleId, CancellationToken cancellationToken);

        Task<bool> HasCertificateAsync(
            int userId, int moduleId, CancellationToken cancellationToken);

        Task IssueCertificateAsync(
            int userId, int moduleId, string certificateNumber, CancellationToken cancellationToken);
    }
}
