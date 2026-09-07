namespace RestaurantTraining.Blazor.Models
{
    
        public class CertificateSettingModel
        {
            public int SettingId { get; set; }
            public int ModuleId { get; set; }
            public bool IsEnabled { get; set; }
            public string Title { get; set; } = string.Empty;
            public string Message { get; set; } = string.Empty;
            public int MinimumPassingScore { get; set; }
            public string Template { get; set; } = string.Empty;
            public string IssuerName { get; set; } = string.Empty;
        }


        public class SaveCertificateSettingRequest
        {
            public int ModuleId { get; set; }
            public bool IsEnabled { get; set; }
            public string Title { get; set; } = string.Empty;
            public string Message { get; set; } = string.Empty;
            public int MinimumPassingScore { get; set; }
            public string Template { get; set; } = string.Empty;
            public string IssuerName { get; set; } = string.Empty;
        }


        public class CertificateSaveResult
        {
            public bool Success { get; set; }
            public string Message { get; set; } = string.Empty;
        }

    
}
