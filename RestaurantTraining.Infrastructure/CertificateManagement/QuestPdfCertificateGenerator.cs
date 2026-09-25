using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using RestaurantTraining.Application.Common.Interfaces;

namespace RestaurantTraining.Infrastructure.CertificateManagement
{
    public class QuestPdfCertificateGenerator : ICertificatePdfGenerator
    {
        private static readonly string BrandBlue = "#2b3a8f";
        private static readonly string DarkNavy = "#1a2340";
        private static readonly string GrayText = "#6b7280";

        public QuestPdfCertificateGenerator()
        {
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public byte[] Generate(CertificatePdfData data)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(0);
                    page.DefaultTextStyle(x => x.FontFamily("Helvetica"));

                    page.Content()
                        .Padding(30)
                        .Border(2)
                        .BorderColor(BrandBlue)
                        .Padding(40)
                        .Column(col =>
                        {
                            col.Spacing(10);

                            // Brand row
                            col.Item().AlignCenter().Row(row =>
                            {
                                row.AutoItem().Height(28).Width(28).Svg($"""
                                    <svg viewBox="0 0 32 32" xmlns="http://www.w3.org/2000/svg">
                                        <circle cx="16" cy="16" r="15" fill="{BrandBlue}" />
                                        <circle cx="16" cy="16" r="8.5" fill="none" stroke="#ffffff" stroke-width="2" />
                                        <circle cx="16" cy="16" r="3" fill="#ffffff" />
                                    </svg>
                                    """);

                                row.AutoItem().PaddingLeft(10).Column(brand =>
                                {
                                    brand.Item().Text("Copperleaf")
                                        .FontSize(16).Bold().FontColor(DarkNavy);
                                    brand.Item().Text("RESTAURANT & HOSPITALITY")
                                        .FontSize(8).Bold().FontColor(BrandBlue)
                                        .LetterSpacing(0.1f);
                                });
                            });

                            col.Item().PaddingTop(15).AlignCenter()
                                .Text("CERTIFICATE OF COMPLETION")
                                .FontSize(14).Bold().FontColor(BrandBlue)
                                .LetterSpacing(0.15f);

                            col.Item().AlignCenter()
                                .Width(60).Height(2).Background(BrandBlue);

                            col.Item().PaddingTop(15).AlignCenter()
                                .Text("This certifies that")
                                .FontSize(12).FontColor(GrayText);

                            col.Item().AlignCenter()
                                .Text(data.LearnerName)
                                .FontSize(30).Bold().FontColor(DarkNavy);

                            col.Item().PaddingTop(8).AlignCenter()
                                .Text("has successfully completed the training module")
                                .FontSize(12).FontColor(GrayText);

                            col.Item().AlignCenter()
                                .Text(data.ModuleName)
                                .FontSize(18).Bold().FontColor(BrandBlue);

                            if (data.ScorePercent is not null)
                            {
                                col.Item().PaddingTop(8).AlignCenter().Text(text =>
                                {
                                    text.Span("with a final score of ")
                                        .FontSize(12).FontColor(GrayText);
                                    text.Span($"{data.ScorePercent}%")
                                        .FontSize(12).Bold().FontColor(DarkNavy);
                                });
                            }

                            col.Item().PaddingTop(20).AlignCenter()
                                .Width(48).Height(64).Svg($"""
                                    <svg viewBox="0 0 48 64" fill="none" xmlns="http://www.w3.org/2000/svg"
                                         stroke="{BrandBlue}" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                                        <path d="M19 32 L14 60 L22 53" />
                                        <path d="M29 32 L34 60 L26 53" />
                                        <circle cx="24" cy="22" r="15" />
                                        <polygon points="24,15 25.76,19.57 30.66,19.84 26.85,22.93 28.11,27.66 24,25 19.89,27.66 21.15,22.93 17.34,19.84 22.24,19.57"
                                                 fill="{BrandBlue}" stroke="none" />
                                    </svg>
                                    """);

                            col.Item().PaddingTop(15).AlignCenter()
                                .Text($"{data.CertificateNumber} · Issued {data.IssuedDate:MMM d, yyyy}")
                                .FontSize(9).FontColor(GrayText);
                        });
                });
            });

            return document.GeneratePdf();
        }
    }
}