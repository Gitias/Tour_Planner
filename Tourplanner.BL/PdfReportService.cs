namespace Tourplanner.BL
{
    using iText.IO.Image;
    using iText.Kernel.Geom;
    using iText.Kernel.Pdf;
    using iText.Layout;
    using iText.Layout.Element;
    using iText.Layout.Properties;
    using Microsoft.Extensions.Configuration;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using Tourplanner.Shared;

    public class PdfReportService : IPdfReportService
    {
        public PdfReportService(ITourLogService tourLogService, IConfiguration configuration)
        {
            this.tourLogService = tourLogService;
            this.imageDirectory = configuration["MapService:ImagePath"]
                ?? throw new ArgumentNullException(nameof(configuration));
        }
        public async Task GenerateSummaryReport(IEnumerable<Tour> tours)
        {
            try
            {
                var pdfDirectory = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "PdfReports");

                if (!Directory.Exists(pdfDirectory))
                {
                    Directory.CreateDirectory(pdfDirectory);
                }

                var pdfPath = System.IO.Path.Combine(pdfDirectory, "Tour_Summary_report.pdf");

                using PdfWriter pdfWriter = new PdfWriter(pdfPath);
                using PdfDocument pdfDocument = new PdfDocument(pdfWriter);
                using Document document = new Document(pdfDocument, PageSize.A4);

                var title = new Paragraph("Tour Summary Report")
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetBold()
                    .SetFontSize(20);

                document.Add(title);

                var table = new Table(4, true)
                    .UseAllAvailableWidth()
                    .SetHorizontalAlignment(HorizontalAlignment.CENTER);

                table.AddHeaderCell("Tour Name");
                table.AddHeaderCell("Avg. Distance");
                table.AddHeaderCell("Avg. Time");
                table.AddHeaderCell("Avg. Rating");

                foreach (var tour in tours)
                {
                    var logs = await tourLogService.GetAllTourLogsFromTourAsync(tour.Id);

                    if (logs != null && logs.Any())
                    {
                        double avgDistance = logs.Average(x => x.Distance);
                        double avgTime = logs.Average(x => x.TotalTime.TotalHours);
                        double avgRating = logs.Average(x => x.Rating);

                        table.AddCell(tour.Name);
                        table.AddCell(avgDistance.ToString("F2"));
                        table.AddCell(avgTime.ToString("F2"));
                        table.AddCell(avgRating.ToString("F1"));
                    }
                    else
                    {
                        table.AddCell(tour.Name);
                        table.AddCell("N/A");
                        table.AddCell("N/A");
                        table.AddCell("N/A");
                    }
                }

                document.Add(table);

                document.Close();
            }
            catch (Exception ex)
            {
            }
        }

        public async Task GenerateTourReport(Tour tour)
        {
            try
            {
                var pdfDirectory = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "PdfReports");

                if (!Directory.Exists(pdfDirectory))
                {
                    Directory.CreateDirectory(pdfDirectory);
                }

                var pdfPath = System.IO.Path.Combine(pdfDirectory, $"{tour.Name}_report.pdf");

                using PdfWriter pdfWriter = new PdfWriter(pdfPath);
                using PdfDocument pdfDocument = new PdfDocument(pdfWriter);
                using Document document = new Document(pdfDocument, PageSize.A4);

                var title = new Paragraph($"Tour Report: {tour.Name}")
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetBold()
                    .SetFontSize(20);

                document.Add(title);

                document.Add(new Paragraph($"Description: {tour.Description}"));
                document.Add(new Paragraph($"From: {tour.From}"));
                document.Add(new Paragraph($"To: {tour.To}"));
                document.Add(new Paragraph($"Transport: {tour.TransportType}"));
                document.Add(new Paragraph($"Popularity: {tour.Popularity}"));
                document.Add(new Paragraph($"Child Friendliness: {tour.ChildFriendliness}"));

                var imagePath = System.IO.Path.Combine(imageDirectory, $"{tour.Name}_map.png");

                if (System.IO.File.Exists(imagePath))
                {
                    var image = new Image(ImageDataFactory.Create(imagePath))
                        .SetAutoScale(true);

                    document.Add(image);
                }

                var logs = await tourLogService.GetAllTourLogsFromTourAsync(tour.Id);

                var tourLogsTitle = new Paragraph("Tour Logs")
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetBold()
                    .SetFontSize(16);

                document.Add(tourLogsTitle);

                if (logs != null && logs.Any())
                {
                    var table = new Table(6)
                        .UseAllAvailableWidth()
                        .SetHorizontalAlignment(HorizontalAlignment.CENTER);

                    table.AddHeaderCell("Date");
                    table.AddHeaderCell("Total Time");
                    table.AddHeaderCell("Distance");
                    table.AddHeaderCell("Rating");
                    table.AddHeaderCell("Difficulty");
                    table.AddHeaderCell("Comment");

                    foreach (var log in logs)
                    {
                        table.AddCell(log.Date.ToString("dd/MM/yyyy"));
                        table.AddCell(log.TotalTime.ToString());
                        table.AddCell(log.Distance.ToString());
                        table.AddCell(log.Rating.ToString());
                        table.AddCell(log.Difficulty);
                        table.AddCell(log.Comment);
                    }

                    document.Add(table);

                }
                else
                {
                    document.Add(new Paragraph("No logs available"));
                }

                document.Close();
            }
            catch (Exception ex)
            {
            }
        }


        private ITourLogService tourLogService;
        private string imageDirectory;
    }
}
