using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using TowBoatSalvageWebApp.Models;
using IContainer = QuestPDF.Infrastructure.IContainer;

namespace TowBoatSalvageWebApp.Services
{
    /// <summary>
    /// Builds a downloadable PDF summary for a single work order.
    /// </summary>
    public sealed class WorkOrderPdfService
    {
        public WorkOrderPdfService()
        {
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public byte[] BuildPdf(WorkOrder order)
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
                        col.Item().Text("Work Order Report")
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

                        // Order info row
                        col.Item().Row(row =>
                        {
                            row.RelativeItem().Column(c =>
                            {
                                c.Item().Text(text =>
                                {
                                    text.Span("Vessel: ").SemiBold();
                                    text.Span(order.VesselName);
                                });
                                c.Item().Text(text =>
                                {
                                    text.Span("Date: ").SemiBold();
                                    text.Span(order.RequestDateDisplay?.ToString("MMMM dd, yyyy") ?? "—");
                                });
                            });

                            row.RelativeItem().Column(c =>
                            {
                                c.Item().Text(text =>
                                {
                                    text.Span("Engine 1 Hours: ").SemiBold();
                                    text.Span(order.Engine1Hours);
                                });
                                c.Item().Text(text =>
                                {
                                    text.Span("Engine 2 Hours: ").SemiBold();
                                    text.Span(order.Engine2Hours);
                                });
                            });

                            row.RelativeItem().Column(c =>
                            {
                                c.Item().Text(text =>
                                {
                                    text.Span("Status: ").SemiBold();
                                    text.Span(order.IsResolved ? "Resolved" : "Open");
                                });
                                c.Item().Text(text =>
                                {
                                    text.Span("Order ID: ").SemiBold();
                                    text.Span($"WO-{order.Id:D4}");
                                });
                            });
                        });

                        col.Item().PaddingVertical(4)
                            .LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

                        // Issues + corrections
                        col.Item().Text("Reported Issues & Corrections")
                            .SemiBold().FontSize(14);

                        if (order.ReportedIssues.Count == 0)
                        {
                            col.Item().Text("No reported issues.")
                                .FontColor(Colors.Grey.Darken1).Italic();
                        }
                        else
                        {
                            for (var i = 0; i < order.ReportedIssues.Count; i++)
                            {
                                var issueIndex = i;
                                var issueText = order.ReportedIssues[issueIndex];

                                // Issue header
                                col.Item().PaddingTop(6).Text(text =>
                                {
                                    text.Span($"Issue #{issueIndex + 1}: ").SemiBold();
                                    text.Span(issueText);
                                });

                                // Correction thread
                                var thread = GetCorrectionThread(order, issueIndex);

                                if (thread.Count == 0)
                                {
                                    col.Item().PaddingLeft(16).Text("No corrections yet.")
                                        .FontColor(Colors.Grey.Darken1).Italic().FontSize(10);
                                }
                                else
                                {
                                    foreach (var correction in thread)
                                    {
                                        col.Item().PaddingLeft(16).Element(CorrectionEntry);

                                        void CorrectionEntry(IContainer c)
                                        {
                                            c.Border(1)
                                             .BorderColor(Colors.Grey.Lighten2)
                                             .Background(Colors.Grey.Lighten4)
                                             .Padding(6)
                                             .Column(inner =>
                                             {
                                                 inner.Item().Row(r =>
                                                 {
                                                     r.RelativeItem().Text(correction.Author)
                                                         .SemiBold().FontSize(10);
                                                     r.AutoItem().Text(
                                                         correction.CreatedAtUtc.ToLocalTime()
                                                             .ToString("M/d/yyyy h:mm tt"))
                                                         .FontSize(9).FontColor(Colors.Grey.Darken1);
                                                 });
                                                 inner.Item().Text(correction.Text).FontSize(10);
                                             });
                                        }
                                    }
                                }
                            }
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
        /// Reads from IssueCorrectionThreads first, falls back to legacy IssueCorrections.
        /// Same logic used in the Blazor pages.
        /// </summary>
        private static List<IssueCorrection> GetCorrectionThread(WorkOrder order, int issueIndex)
        {
            if (order.IssueCorrectionThreads?.TryGetValue(issueIndex, out var thread) == true
                && thread.Count > 0)
            {
                return thread;
            }

            if (order.IssueCorrections is not null
                && issueIndex < order.IssueCorrections.Count
                && !string.IsNullOrWhiteSpace(order.IssueCorrections[issueIndex]))
            {
                var legacyAuthor = order.IssueCorrectionsBy is not null
                    && issueIndex < order.IssueCorrectionsBy.Count
                        ? order.IssueCorrectionsBy[issueIndex]
                        : "Unknown";

                return
                [
                    new IssueCorrection
                    {
                        Author = legacyAuthor,
                        Text = order.IssueCorrections[issueIndex],
                        CreatedAtUtc = order.RequestDateDisplay?.ToUniversalTime() ?? DateTime.UtcNow
                    }
                ];
            }

            return [];
        }
    }
}