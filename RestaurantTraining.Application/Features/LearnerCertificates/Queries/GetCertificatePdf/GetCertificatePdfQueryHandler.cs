using MediatR;
using RestaurantTraining.Application.Common.Interfaces;

namespace RestaurantTraining.Application.Features.LearnerCertificates.Queries.GetCertificatePdf
{
    public class GetCertificatePdfQueryHandler
        : IRequestHandler<GetCertificatePdfQuery, CertificatePdfResult?>
    {
        private readonly ILearnerCertificateRepository _repository;
        private readonly ICertificatePdfGenerator _pdfGenerator;

        public GetCertificatePdfQueryHandler(
            ILearnerCertificateRepository repository,
            ICertificatePdfGenerator pdfGenerator)
        {
            _repository = repository;
            _pdfGenerator = pdfGenerator;
        }

        public async Task<CertificatePdfResult?> Handle(
            GetCertificatePdfQuery request, CancellationToken cancellationToken)
        {
            var cert = await _repository.GetCertificateAsync(
                request.CertificateId, request.UserId, cancellationToken);

            if (cert is null) return null; // not found OR not this learner's cert

            var learnerName = await _repository.GetLearnerNameAsync(
                request.UserId, cancellationToken);

            var attempts = await _repository.GetPassedAttemptsAsync(
                request.UserId, new List<int> { cert.ModuleId }, cancellationToken);

            decimal? scorePercent = attempts
                .Where(a => a.ModuleId == cert.ModuleId && a.TotalMarks > 0)
                .Select(a => (decimal?)Math.Round(a.Score / a.TotalMarks * 100, 0))
                .OrderByDescending(s => s)
                .FirstOrDefault();

            var pdfData = new CertificatePdfData
            {
                LearnerName = learnerName,
                ModuleName = cert.ModuleName,
                CertificateNumber = cert.CertificateNumber,
                IssuedDate = cert.IssuedDate,
                ScorePercent = scorePercent
            };

            var bytes = _pdfGenerator.Generate(pdfData);
            var fileName = $"Certificate_{cert.ModuleName}_{cert.CertificateNumber}.pdf"
                .Replace(" ", "_");

            return new CertificatePdfResult { FileBytes = bytes, FileName = fileName };
        }
    }
}