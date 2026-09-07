namespace RestaurantTraining.Domain.Entities
{
    public class Certificate
    {
        public int CertificateId { get; set; }

        public int UserId { get; set; }

        public int ModuleId { get; set; }

        public string CertificateNumber { get; set; } = string.Empty;

        public DateTime IssuedDate { get; set; }
    }
}