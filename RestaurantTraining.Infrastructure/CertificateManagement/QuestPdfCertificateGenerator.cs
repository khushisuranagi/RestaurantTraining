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
                                row.AutoItem().Height(28).Width(28)
                                    .Background(BrandBlue)
                                    .Container();

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
                                .Width(46).Height(46)
                                .Background(BrandBlue)
                                .Container();

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