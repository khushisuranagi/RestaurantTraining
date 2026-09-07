using MediatR;
using RestaurantTraining.Application.Common.Interfaces;

namespace RestaurantTraining.Application.Features.LearnerCertificates.Queries.GetMyCertificates
{
    public class GetMyCertificatesQueryHandler
        : IRequestHandler<GetMyCertificatesQuery, List<LearnerCertificateDto>>
    {
        private readonly ILearnerCertificateRepository _repository;

        public GetMyCertificatesQueryHandler(
            ILearnerCertificateRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<LearnerCertificateDto>> Handle(
            GetMyCertificatesQuery request,
            CancellationToken cancellationToken)
        {
            var learnerName = await _repository.GetLearnerNameAsync(
                request.UserId, cancellationToken);

            var certs = await _repository.GetCertificatesAsync(
                request.UserId, cancellationToken);

            var moduleIds = certs.Select(c => c.ModuleId).ToList();

            var attempts = await _repository.GetPassedAttemptsAsync(
                request.UserId, moduleIds, cancellationToken);

            // Best passing score per module (percentage).
            var bestByModule = attempts
                .GroupBy(a => a.ModuleId)
                .ToDictionary(
                    g => g.Key,
                    g => g.Max(a => a.TotalMarks > 0
                        ? Math.Round(a.Score / a.TotalMarks * 100, 0)
                        : 0));

            return certs.Select(c => new LearnerCertificateDto
            {
                CertificateId = c.CertificateId,
                CertificateNumber = c.CertificateNumber,
                ModuleName = c.ModuleName,
                IssuedDate = c.IssuedDate,
                LearnerName = learnerName,
                ScorePercent = bestByModule.TryGetValue(c.ModuleId, out var s)
                    ? (decimal?)s
                    : null
            }).ToList();
        }
    }
}
