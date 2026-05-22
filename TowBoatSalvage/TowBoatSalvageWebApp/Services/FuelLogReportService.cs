using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace TowBoatSalvageWebApp.Services
{
    public sealed class FuelLogReportService
    {
        private readonly FuelLogService _fuelLogService;


        public FuelLogReportService(FuelLogService fuelLogService)
        {
            _fuelLogService = fuelLogService;
            QuestPDF.Settings.License = LicenseType.Community;

        }



        public async Task<byte[]> BuildPdfAsync(string boatName, int year)
        {
            var entries = await _fuelLogService.GetEntriesForBoatYearAsync(boatName, year);

            var totalFuel = entries.Sum(e => e.Fuel1 + e.Fuel2 + e.GasCans);
            var totalFuel1 = entries.Sum(e => e.Fuel1);
            var totalFuel2 = entries.Sum(e => e.Fuel2);
            var totalGasCans = entries.Sum(e => e.GasCans);

            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);
                    page.Size(PageSizes.Letter);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Header().Column(col =>
                    {
                        col.Item().Text("Fuel Log Report").FontSize(16).Bold();
                        col.Item().Text($"Boat: {boatName}");
                        col.Item().Text($"Year: {year}");
                        col.Item().Text($"Generated: {ToEastern(DateTime.UtcNow):yyyy-MM-dd HH:mm} ET");
                    });

                    page.Content().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(70);
                            columns.RelativeColumn();
                            columns.ConstantColumn(60);
                            columns.ConstantColumn(60);
                            columns.ConstantColumn(60);
                            columns.ConstantColumn(70);
                            columns.ConstantColumn(110);
                        });

                        table.Header(header =>
                        {
                            header.Cell().Element(HeaderCell).Text("Date");
                            header.Cell().Element(HeaderCell).Text("Crew");
                            header.Cell().Element(HeaderCell).Text("Eng1");
                            header.Cell().Element(HeaderCell).Text("Eng2");
                            header.Cell().Element(HeaderCell).Text("Fuel1");
                            header.Cell().Element(HeaderCell).Text("Fuel2");
                            header.Cell().Element(HeaderCell).Text("Gas Cans");
                        });

                        foreach (var e in entries)
                        {
                            table.Cell().Element(Cell).Text(e.LogDate.ToString("yyyy-MM-dd"));
                            table.Cell().Element(Cell).Text(e.CrewMember);
                            table.Cell().Element(Cell).Text(e.Engine1Hours.ToString("0.##"));
                            table.Cell().Element(Cell).Text(e.Engine2Hours.ToString("0.##"));
                            table.Cell().Element(Cell).Text(e.Fuel1.ToString("0.##"));
                            table.Cell().Element(Cell).Text(e.Fuel2.ToString("0.##"));
                            table.Cell().Element(Cell).Text(e.GasCans.ToString("0.##"));
                        }

                        table.Cell().ColumnSpan(7).PaddingTop(10).Text("Totals").Bold();
                        table.Cell().Element(Cell).Text("");
                        table.Cell().Element(Cell).Text("");
                        table.Cell().Element(Cell).Text("");
                        table.Cell().Element(Cell).Text("");
                        table.Cell().Element(Cell).Text(totalFuel1.ToString("0.##")).Bold();
                        table.Cell().Element(Cell).Text(totalFuel2.ToString("0.##")).Bold();
                        table.Cell().Element(Cell).Text(totalGasCans.ToString("0.##")).Bold();

                        table.Cell().ColumnSpan(7).AlignRight().PaddingTop(4)
                            .Text($"Total Fuel (incl. gas cans): {totalFuel:0.##}").Bold();
                    });

                    page.Footer().AlignCenter()
                        .Text("Fuel log report for internal accounting and tax documentation.");
                });
            }).GeneratePdf();
        }

            private static IContainer HeaderCell(IContainer container) =>
            container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(4).BorderBottom(1).BorderColor(Colors.Grey.Lighten2);

        private static IContainer Cell(IContainer container) =>
            container.PaddingVertical(3).BorderBottom(1).BorderColor(Colors.Grey.Lighten3);

        private static DateTime ToEastern(DateTime utc)
        {
            try
            {
                var tz = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                return TimeZoneInfo.ConvertTimeFromUtc(utc, tz);
            }
            catch
            {
                var tz = TimeZoneInfo.FindSystemTimeZoneById("America/New_York");
                return TimeZoneInfo.ConvertTimeFromUtc(utc, tz);
            }
        }

    }
    
}
