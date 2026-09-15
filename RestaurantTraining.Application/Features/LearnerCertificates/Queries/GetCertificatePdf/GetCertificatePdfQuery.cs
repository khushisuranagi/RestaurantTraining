using MediatR;

namespace RestaurantTraining.Application.Features.LearnerCertificates.Queries.GetCertificatePdf
{
    public class GetCertificatePdfQuery : IRequest<CertificatePdfResult?>
    {
        public int CertificateId { get; set; }
        public int UserId { get; set; }
    }

    public class CertificatePdfResult
    {
        public byte[] FileBytes { get; set; } = Array.Empty<byte>();
        public string FileName { get; set; } = string.Empty;
    }
}