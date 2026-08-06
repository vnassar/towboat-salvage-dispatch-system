using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using TowBoatSalvageWebApp.Models;
using IContainer = QuestPDF.Infrastructure.IContainer;

namespace TowBoatSalvageWebApp.Services
{
    /// <summary>
    /// Builds a downloadable PDF summary for a single vehicle inspection
    /// </summary>
    public sealed class VehicleInspectionPdfService
    {
        public VehicleInspectionPdfService()
        {
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public byte[] BuildPdf(VesselInspection inspection)
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
                        col.Item().Text("Vessel Inspection")
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
                                    text.Span(inspection.BoatNumber);
                                });
                                c.Item().Text(text =>
                                {
                                    text.Span("Completed By: ").SemiBold();
                                    text.Span(inspection.CompletedBy);
                                });
                            });

                            row.RelativeItem().Column(c =>
                            {
                                c.Item().Text(text =>
                                {
                                    text.Span("Date of Inspection: ").SemiBold();
                                    text.Span(inspection.DateOfInspection?.ToString("MMMM dd, yyyy") ?? "—");
                                });
                                c.Item().Text(text =>
                                {
                                    text.Span("Inspection ID: ").SemiBold();
                                    text.Span($"{inspection.Id}");
                                });
                            });
                        });

                        col.Item().PaddingVertical(4)
                            .LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

                        // ── Service items ──
                        if (inspection.ServiceDescriptions != null && inspection.ServiceDescriptions.Any())
                        {
                            col.Item().Text("Inspection Items").SemiBold().FontSize(14);

                            col.Item().Column(inner =>
                            {
                                inner.Spacing(2);
                                foreach (var service in inspection.ServiceDescriptions)
                                {
                                    inner.Item().Row(r =>
                                    {
                                        r.AutoItem().Text(service.bServiceCompleted ? "✓" : "✗")
                                            .FontSize(12)
                                            .FontColor(service.bServiceCompleted ? Colors.Green.Medium : Colors.Red.Medium)
                                            .AlignCenter();
                                        r.RelativeItem().Text($"  {service.Description}").FontSize(10);
                                    });

                                    // ── Date row for completed items ──
                                    if (service.bServiceCompleted && service.DateForThisItem.HasValue && !service.Description.Contains("Date of completed pump check", StringComparison.OrdinalIgnoreCase))
                                    {
                                        inner.Item().PaddingLeft(24).Text(text =>
                                        {
                                            text.Span("Expires: ").FontColor(Colors.Grey.Darken1).FontSize(9);
                                            text.Span(service.DateForThisItem.Value.ToString("MMMM dd, yyyy"))
                                                .FontColor(Colors.Grey.Darken2).FontSize(9);
                                        });
                                    }


                                    if (service.bServiceCompleted && service.DateForThisItem.HasValue && service.Description.Contains("Date of completed pump check", StringComparison.OrdinalIgnoreCase))
                                    {
                                        inner.Item().PaddingLeft(24).Text(text =>
                                        {
                                            text.Span("Date: ").FontColor(Colors.Grey.Darken1).FontSize(9);
                                            text.Span(service.DateForThisItem.Value.ToString("MMMM dd, yyyy"))
                                                .FontColor(Colors.Grey.Darken2).FontSize(9);
                                        });
                                    }

                                    if (service.bServiceCompleted && service.FirstRecording.HasValue && service.SecondRecording.HasValue&& service.Description.Contains("Check DVR Card for recording on pc", StringComparison.OrdinalIgnoreCase))
                                    {
                                        inner.Item().PaddingLeft(24).Text(text =>
                                        {
                                            text.Span("First Recording: ").FontColor(Colors.Grey.Darken1).FontSize(9);
                                            text.Span(service.FirstRecording!.Value.ToString("MMMM dd, yyyy"))
                                                .FontColor(Colors.Grey.Darken2).FontSize(9);
                                        });

                                        inner.Item().PaddingLeft(24).Text(text =>
                                        {
                                            text.Span("Second Recording: ").FontColor(Colors.Grey.Darken1).FontSize(9);
                                            text.Span(service.SecondRecording!.Value.ToString("MMMM dd, yyyy"))
                                                .FontColor(Colors.Grey.Darken2).FontSize(9);
                                        });
                                    }
                                }
                            });
                        }

                        // ── Notes ──
                        if (!string.IsNullOrWhiteSpace(inspection.Notes))
                        {
                            col.Item().PaddingTop(6)
                                .LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
                            col.Item().Text("Notes").SemiBold().FontSize(14);
                            col.Item().Text(inspection.Notes).FontSize(10).FontColor(Colors.Grey.Darken2);
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

        /// <summary>
        /// Renders a section with a title and a two-column checklist of items.
        /// </summary>
       private static void RenderSection(ColumnDescriptor col, string title, object section, (string Label, object? Value)[] items)
        {
            col.Item().Text(title).SemiBold().FontSize(14);

            col.Item().Column(inner =>
            {
                inner.Spacing(2);
                foreach (var (label, value) in items)
                {
                    inner.Item().Row(r =>
                    {
                        r.RelativeItem().Text(label).FontSize(10);
                        r.AutoItem().Text(value is bool isChecked ? (isChecked ? "✓" : "✗") : (value?.ToString() ?? "—"))
                            .FontSize(10)
                            .FontColor(value is bool isTrue && !isTrue ? Colors.Red.Medium : Colors.Grey.Darken2);
                    });
                }
            });
        }
    }
}