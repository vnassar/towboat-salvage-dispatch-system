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

        public byte[] BuildPdf(VehicleInspection inspection)
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
                        col.Item().Text("AMS Vessel Equipment Inspection Checklist")
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

                        // ── Inspection info row ──
                        col.Item().Row(row =>
                        {
                            row.RelativeItem().Column(c =>
                            {
                                c.Item().Text(text =>
                                {
                                    text.Span("Vessel: ").SemiBold();
                                    text.Span(inspection.VesselNumber);
                                });
                                c.Item().Text(text =>
                                {
                                    text.Span("Employee/Captain: ").SemiBold();
                                    text.Span(inspection.EmployeeOrCaptain);
                                });
                            });

                            row.RelativeItem().Column(c =>
                            {
                                c.Item().Text(text =>
                                {
                                    text.Span("Date: ").SemiBold();
                                    text.Span(inspection.DateOfInspection?.ToString("MMMM dd, yyyy") ?? "—");
                                });
                                c.Item().Text(text =>
                                {
                                    text.Span("Inspection ID: ").SemiBold();
                                    text.Span($"{inspection.Id:D4}");
                                });
                            });
                        });

                        col.Item().PaddingVertical(4)
                            .LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

                        // ── Sections ──
                        RenderSection(col, "Initial Inspection", inspection.InitialInspection,
                            new (string, object?)[]
                            {
                                ("Florida DOC Certificate", inspection.InitialInspection.FloridaDOCCertificate),
                                ("Florida Registration", inspection.InitialInspection.FloridaRegistration),
                                ("Current Registration", inspection.InitialInspection.CurrentRegistration),
                                ("Alcohol Test Strip", inspection.InitialInspection.AlcoholTestStrip),
                                ("Alcohol Test Strip Date", inspection.InitialInspection.AlcoholTestStripDate?.ToString("M/d/yyyy")),
                                ("Vessel's Log Book", inspection.InitialInspection.VesselsLogBook),
                            });

                        RenderSection(col, "Captain's Personal Items", inspection.CaptainsPersonalItems,
                            new (string, object?)[]
                            {
                                ("Captain's USCG License", inspection.CaptainsPersonalItems.CaptainsUSCGLicence),
                                ("USCG License Expires", inspection.CaptainsPersonalItems.USCGLicenseExpires?.ToString("M/d/yyyy")),
                                ("Proof of Drug Consortium", inspection.CaptainsPersonalItems.ProofOfDrugConsortium),
                                ("Drug Consortium Expires", inspection.CaptainsPersonalItems.DrugConsortiumExpires?.ToString("M/d/yyyy")),
                                ("Auto-Inflate Life Jacket", inspection.CaptainsPersonalItems.AutoInflateLifeJacket),
                                ("Life Jacket Expires", inspection.CaptainsPersonalItems.AutoInflateLifeJacketExpires?.ToString("M/d/yyyy")),
                                ("Captain's Ditch Bag", inspection.CaptainsPersonalItems.CaptainsDitchBag),
                                ("PLB Battery Registration", inspection.CaptainsPersonalItems.PLBBatteryRegistration),
                                ("PLB Expires", inspection.CaptainsPersonalItems.PLBExpires?.ToString("M/d/yyyy")),
                                ("Clipboards with Invoices", inspection.CaptainsPersonalItems.ClipboardsWithInvoices),
                                ("USCG Rules of the Road Book", inspection.CaptainsPersonalItems.USCGRulesOfTheRoadBook),
                            });

                        RenderSection(col, "Safety Devices", inspection.SafetyDevices,
                            new (string, object?)[]
                            {
                                ("Captain/Crew Type 1 Offshore", inspection.SafetyDevices.CaptainCrewType1Offshore),
                                ("Adult PFDs", inspection.SafetyDevices.AdultPFDs),
                                ("Child's PFDs", inspection.SafetyDevices.ChildsPFDs),
                                ("Throwable Life Ring", inspection.SafetyDevices.ThrowableLifeRing),
                                ("Cold Water Suite", inspection.SafetyDevices.ColdWaterSuite),
                                ("Flares", inspection.SafetyDevices.Flares),
                                ("Flares Expire", inspection.SafetyDevices.FlaresExpire?.ToString("M/d/yyyy")),
                                ("EPIRB", inspection.SafetyDevices.Epirb),
                                ("EPIRB Expires", inspection.SafetyDevices.EpirbExpires?.ToString("M/d/yyyy")),
                                ("Fire Extinguishers", inspection.SafetyDevices.FireExtinguishers),
                                ("First Aid Kit", inspection.SafetyDevices.FirstAidKit),
                                ("Thermal Blanket", inspection.SafetyDevices.ThermalBlanket),
                                ("Handheld VHF", inspection.SafetyDevices.HandheldVHF),
                            });

                        RenderSection(col, "Mounted Lights", inspection.MountedLights,
                            new (string, object?)[]
                            {
                                ("Navigation", inspection.MountedLights.Navigation),
                                ("Towing", inspection.MountedLights.Towing),
                                ("Public Safety", inspection.MountedLights.PublicSafety),
                                ("Spot Light", inspection.MountedLights.SpotLight),
                                ("Deck Flood", inspection.MountedLights.DeckFlood),
                                ("Flashlight", inspection.MountedLights.Flashlight),
                                ("Horn", inspection.MountedLights.Horn),
                                ("Siren", inspection.MountedLights.Siren),
                                ("Fog Horn", inspection.MountedLights.FogHorn),
                                ("Hailer", inspection.MountedLights.Hailer),
                            });

                        RenderSection(col, "Placards", inspection.Placards,
                            new (string, object?)[]
                            {
                                ("Discharge of Oil", inspection.Placards.DischargeOfOil),
                                ("Discharge of Garbage", inspection.Placards.DischargeOfGarbage),
                                ("Navigation Chart", inspection.Placards.NavigationChart),
                                ("BoatUS Membership Applications", inspection.Placards.BoatUSMembershipApplications),
                            });

                        RenderSection(col, "Navigation Equipment", inspection.Navigation,
                            new (string, object?)[]
                            {
                                ("GPS", inspection.Navigation.GPS),
                                ("Backup GPS", inspection.Navigation.BackupGPS),
                                ("Depth Finder", inspection.Navigation.DepthFinder),
                                ("Radar", inspection.Navigation.Radar),
                                ("VHF Primary", inspection.Navigation.VHFPrimary),
                                ("VHF Secondary", inspection.Navigation.VHFSecondary),
                                ("DVR", inspection.Navigation.DVR),
                                ("DVR Card Recording", inspection.Navigation.CheckDVRCardForRecording),
                                ("First Recording", inspection.Navigation.FirstRecording?.ToString("M/d/yyyy")),
                                ("Last Recording", inspection.Navigation.LastRecording?.ToString("M/d/yyyy")),
                                ("Whiskey/Electronic Compass", inspection.Navigation.WhiskeyOrELectronicCompass),
                                ("Binoculars", inspection.Navigation.Binoculars),
                            });

                        RenderSection(col, "Vessel Equipment", inspection.VesselEquipment,
                            new (string, object?)[]
                            {
                                ("Tow Bit", inspection.VesselEquipment.TowBit),
                                ("Knife", inspection.VesselEquipment.Knife),
                                ("Primary Tow Line", inspection.VesselEquipment.PrimaryTowLine),
                                ("Secondary Tow Line", inspection.VesselEquipment.SecondaryTowLine),
                                ("Four Dock Lines", inspection.VesselEquipment.FourDockLines),
                                ("Fenders (2x8)", inspection.VesselEquipment.FendersTwo8),
                                ("Spare Battery", inspection.VesselEquipment.SpareBattery),
                                ("Jumper Cables", inspection.VesselEquipment.JumperCables),
                                ("Two Bilge Pumps", inspection.VesselEquipment.TwoBilgePumps),
                                ("Primary Anchor", inspection.VesselEquipment.PrimaryAnchor),
                                ("Backup Anchor", inspection.VesselEquipment.BackupAnchor),
                                ("Boat Hook", inspection.VesselEquipment.BoatHook),
                                ("Tool Kit", inspection.VesselEquipment.ToolKit),
                                ("Two Gasoline Cans", inspection.VesselEquipment.TwoGasolineCans),
                                ("Two Fuel Filters", inspection.VesselEquipment.TwoFuelFilters),
                            });

                        RenderSection(col, "Salvage Equipment", inspection.Salvage,
                            new (string, object?)[]
                            {
                                ("Two Rule Pumps", inspection.Salvage.TwoRulePumps),
                                ("Bucket", inspection.Salvage.Bucket),
                                ("Pump", inspection.Salvage.Pump),
                                ("PVC Pipe", inspection.Salvage.PVCPipe),
                                ("Suction Hose", inspection.Salvage.SuctionHose),
                                ("Pads", inspection.Salvage.Pads),
                                ("Wooden Plugs", inspection.Salvage.WoodenPlugs),
                                ("Foam Footballs", inspection.Salvage.FoamFootballs),
                            });

                        RenderSection(col, "Oil", inspection.Oil,
                            new (string, object?)[]
                            {
                                ("Two-Stroke Oil", inspection.Oil.TwoStrokOil),
                                ("Four-Cycle Oil", inspection.Oil.FourCycleOil),
                            });

                        RenderSection(col, "Operational Check", inspection.OperationalCheck,
                            new (string, object?)[]
                            {
                                ("Salvage Pump", inspection.OperationalCheck.SalvagePump),
                                ("Service Pump", inspection.OperationalCheck.ServicePump),
                                ("Empty Fuel Carburetor", inspection.OperationalCheck.EmptyFuelCarburator),
                                ("Fill Gas Tank", inspection.OperationalCheck.FillGasTank),
                                ("Inspect Wiring", inspection.OperationalCheck.InspectWiring),
                                ("Verify Pump Runs", inspection.OperationalCheck.VerifyPumpRuns),
                                ("Date of Pump Check", inspection.OperationalCheck.DateOfPumpCheck?.ToString("M/d/yyyy")),
                            });
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