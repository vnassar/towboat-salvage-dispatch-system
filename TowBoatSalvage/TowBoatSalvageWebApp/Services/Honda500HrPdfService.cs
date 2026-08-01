using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using TowBoatSalvageWebApp.Models;

namespace TowBoatSalvageWebApp.Services
{
    /// <summary>
    /// Builds a downloadable PDF summary for a Honda 500-Hour Service record
    /// </summary>
    public sealed class Honda500HrPdfService
    {
        public Honda500HrPdfService()
        {
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public byte[] BuildPdf(Honda500Hr honda)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.Letter);
                    page.Margin(30);
                    page.DefaultTextStyle(x => x.FontSize(11));

                    // ── Header ──
                    page.Header().Column(col =>
                    {
                        col.Spacing(2);
                        col.Item().Text("Honda 500-Hour Service")
                            .SemiBold().FontSize(18).AlignCenter();
                        col.Item().Text("American Marine Services, LLC")
                            .FontSize(10).FontColor(Colors.Grey.Darken1).AlignCenter();
                        col.Item().PaddingBottom(8)
                            .LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
                    });

                    // ── Content ──
                    page.Content().Column(col =>
                    {
                        col.Spacing(6);

                        // ── Vessel info row ──
                        col.Item().Row(row =>
                        {
                            row.RelativeItem().Column(c =>
                            {
                                c.Item().Text(text =>
                                {
                                    text.Span("Boat Number: ").SemiBold();
                                    text.Span(honda.BoatNumber);
                                });
                                c.Item().Text(text =>
                                {
                                    text.Span("Completed By: ").SemiBold();
                                    text.Span(honda.CompletedBy);
                                });
                            });

                            row.RelativeItem().Column(c =>
                            {
                                c.Item().Text(text =>
                                {
                                    text.Span("Date Completed: ").SemiBold();
                                    text.Span(honda.DateCompleted.ToString("MMMM dd, yyyy"));
                                });
                                c.Item().Text(text =>
                                {
                                    text.Span("Service ID: ").SemiBold();
                                    text.Span($"{honda.Id}");
                                });
                            });
                        });

                        // ── Engine hours ──
                        col.Item().Column(inner =>
                        {
                            inner.Item().Text("Engine Hours").SemiBold().FontSize(14);
                            inner.Item().Row(r =>
                            {
                                r.RelativeItem().Column(c =>
                                {
                                    c.Item().Text("Engine 1").FontSize(10).FontColor(Colors.Grey.Darken2);
                                    c.Item().Text($"{honda.EngineHours1} hrs").SemiBold().FontSize(14);
                                });
                                r.RelativeItem().Column(c =>
                                {
                                    c.Item().Text("Engine 2").FontSize(10).FontColor(Colors.Grey.Darken2);
                                    c.Item().Text($"{honda.EngineHours2} hrs").SemiBold().FontSize(14);
                                });
                            });
                        });

                        col.Item().PaddingVertical(4)
                            .LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

                        // ── Service items ──
                        if (honda.ServiceDescriptions != null && honda.ServiceDescriptions.Any())
                        {
                            col.Item().Text("Service Items").SemiBold().FontSize(14);

                            col.Item().Column(inner =>
                            {
                                inner.Spacing(2);
                                foreach (var service in honda.ServiceDescriptions)
                                {
                                    inner.Item().Row(r =>
                                    {
                                        r.AutoItem().Text(service.bServiceCompleted ? "✓" : "✗")
                                            .FontSize(12)
                                            .FontColor(service.bServiceCompleted ? Colors.Green.Medium : Colors.Red.Medium)
                                            .AlignCenter();
                                        r.RelativeItem().Text($"  {service.Description}").FontSize(10);
                                    });
                                }
                            });
                        }
                    });

                    // ── Footer ──
                    page.Footer().AlignCenter().Text(text =>
                    {
                        text.Span($"Generated {DateTime.Now:M/d/yyyy h:mm tt}  •  ")
                            .FontSize(9).FontColor(Colors.Grey.Darken1);
                        text.Span("Page ");
                        text.CurrentPageNumber();
                        text.Span(" of ");
                        text.TotalPages();
                    });
                });
            });

            return document.GeneratePdf();
        }
    }
}