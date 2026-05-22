using System.Collections.Concurrent;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Companion;
using QuestPDF.Previewer;
using System.ComponentModel;
using IContainer = QuestPDF.Infrastructure.IContainer;
using TowBoatSalvageWebApp.Models;

namespace TowBoatSalvageWebApp.Services
{
    public sealed class DispatchSignedPdfService
    {
        private readonly ConcurrentDictionary<string, byte[]> _pdfStore = new();

        public DispatchSignedPdfService()
        {
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public byte[] BuildSignedPdf(DispatchSignedPdfModel model)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Header()
                        .Text($"American Marine Services, LLC\n13621 Walsingham Road\nLargo, Florida 33774\n\n{model.FormName}\n")
                        .SemiBold()
                        .AlignCenter()
                        .FontSize(16);

                    page.Content().Column(col =>
                    {
                        col.Spacing(8);
                        col.Item().PaddingTop(50).Text(text =>
                        {
                            text.Span("I, ");
                            text.Span($"    {model.CustomerName}    ").Underline();
                            text.Span("owner or owner's agent for the vessel");
                            text.Span($"    {model.VesselName}    ").Underline();
                            text.Span("described as a Length");
                            text.Span($"    {model.VesselLength}    ,").Underline();
                            text.Span($"    {model.VesselMakeModek}   ,").Underline();
                            if (model.FormName.Contains("Ungrounding"))
                            {
                                text.Span(" request American Marine Services, LLC DBA TowBoatUS to unground my vessel located at");
                                text.Span($"    {model.GPS}    .").Underline();
                            }
                            else if (model.FormName.Contains("Hazardous"))
                            {
                                text.Span(" request American Marine Services, LLC DBA TowBoatUS to rescue my vessel located at");
                                text.Span($"    {model.GPS}    .").Underline();
                            }
                            else if (model.FormName.Contains("Unaccompanied"))
                            {
                                text.Span(" free acknowledge that I will not accompany the vessel on a tow from");
                                text.Span($"    {model.UnaccompaniedOrigin}    ").Underline();
                                text.Span("to");
                                text.Span($"    {model.UnaccompaniedDestination}    .").Underline();
                            }
                            else if (model.FormName.Contains("Mooring"))
                            {
                                text.Span(" vessel poisition:");
                                text.Span($"    {model.GPS}    ,").Underline();
                            }

                        });
                        col.Item().Text($"{model.DocumentContent}");

                        col.Item().PaddingVertical(8)
                            .LineHorizontal(1)
                            .LineColor(Colors.Grey.Lighten2);


                        //**********
                        col.Item().Row(row =>
                        {

                            row.RelativeItem().AlignCenter().PaddingTop(100).Text(text =>
                            {
                                //customer section
                                text.Span("Customer Electronic Signature").SemiBold();
                            });
                        });

                        col.Item().Row(row =>
                        {
                            // date & email
                            row.RelativeItem().Text(text =>
                            {
                                text.Span("Signed At:");
                                text.Span($"    {model.SignedAtUtc.ToLocalTime().ToString("g")}    ").Underline();
                            });

                            row.RelativeItem().Text(text =>
                            {
                                text.Span("Email Address:");
                                text.Span($"    {model.CustomerEmail}    ").Underline();
                            });

                        });

                        col.Item().Row(row =>
                        {
                            //number and typed signature
                            row.RelativeItem().Text(text =>
                            {
                                text.Span("Phone Number:");
                                text.Span($"    {model.CustomerPhoneNumber}    ").Underline();
                            });

                            row.RelativeItem().Text(text =>
                            {
                                text.Span("Typed Signature");
                                text.Span($"    {model.Signature}    ").Underline();
                            });

                        });

                        //*************
                        col.Item().Row(row =>
                        {

                            row.RelativeItem().AlignCenter().Text(text =>
                            {
                                //Captain section
                                text.Span("TowBoatUS Captain").SemiBold();
                            });
                        });

                        col.Item().Row(row =>
                        {
                            
                            row.RelativeItem().Text(text =>
                            {
                                text.Span("Captain Name");
                                text.Span($"    {model.CaptainName}    ").Underline();
                            });

                            row.RelativeItem().Text(text =>
                            {
                                text.Span("Captain Typed Signature");
                                text.Span($"    {model.CaptainSignature}    ").Underline();
                            });

                        });
                    });
                });
            });

            

            var bytes = document.GeneratePdf();

            _pdfStore[model.Token] = bytes;
            return bytes;
        }

        public byte[] BuildCreditCardAuthorizationPdf(DispatchSignedPdfModel model)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Header()
                        .Text($"American Marine Services, LLC\n13621 Walsingham Road\nLargo, Florida 33774\n\n{model.FormName}\n")
                        .SemiBold()
                        .AlignCenter()
                        .FontSize(16);

                    page.Content().Column(col =>
                    {
                        col.Spacing(8);
                        col.Item().PaddingTop(50).Text(text =>
                        {
                            //text.Span("I, ");
                            //text.Span($"    {model.CustomerName}    ").Underline();
                            //text.Span("owner or owner's agent for the vessel");
                            //text.Span($"    {model.VesselName}    ").Underline();
                            //text.Span("described as a Length");
                            //text.Span($"    {model.VesselLength}    ,").Underline();
                            //text.Span($"    {model.VesselMakeModek}   ,").Underline();
                            if (model.FormName.Contains("Ungrounding"))
                            {
                                text.Span(" request American Marine Services, LLC DBA TowBoatUS to unground my vessel located at");
                                text.Span($"    {model.GPS}    .").Underline();
                            }
                            else if (model.FormName.Contains("Hazardous"))
                            {
                                text.Span(" request American Marine Services, LLC DBA TowBoatUS to rescue my vessel located at");
                                text.Span($"    {model.GPS}    .").Underline();
                            }
                            else if (model.FormName.Contains("Unaccompanied"))
                            {
                                text.Span(" free acknowledge that I will not accompany the vessel on a tow from");
                                text.Span($"    {model.UnaccompaniedOrigin}    ").Underline();
                                text.Span("to");
                                text.Span($"    {model.UnaccompaniedDestination}    .").Underline();
                            }
                            else if (model.FormName.Contains("Mooring"))
                            {
                                text.Span(" vessel poisition:");
                                text.Span($"    {model.GPS}    ,").Underline();
                            }
                            else if (model.FormName.Contains("Credit"))
                            {
                                text.Span("You authorize a single charge to your credit or debit card. You will be charged the amount indicated below. A receipt for this payment will be provided to you and the charge will appear on your credit card or bank statement.");
                            }

                        });
                        col.Item().PaddingTop(5).Text(text =>
                        {
                            text.Span("Authorized amount:");
                            text.Span($"    ${model.Quote}    .").Underline();
                        });

                        col.Item().PaddingTop(5).Text(text =>
                        {
                            text.Span($"I,");
                            text.Span($"    {model.CustomerName}    ,").Underline();
                            text.Span($" authorize American Marine Services LLC / TowBoatU.S Greater Tampa Bay to charge my Credit/Debit Card.");
                        });
                        col.Item().PaddingTop(5).Text(text =>
                        {
                            text.Span($"This payment is for the following: ");
                            text.Span($"Towing / Salvage services provided.").Underline();
                        });
                        //col.Item().Text($"Authorized Amount:");

                        col.Item().PaddingTop(10).Element(CorrectionEntry);

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
                                     r.RelativeItem().Text($"Card Number: {model.CardNumber}")
                                         .SemiBold().FontSize(12);
                                     r.AutoItem().Text($"Card Type: {model.CardType}")
                                         .FontSize(9).FontColor(Colors.Grey.Darken1);
                                 });

                                 inner.Item().Text($"Expiration: {model.Expiration}").FontSize(12);
                                 inner.Item().Text($"CVV: {model.CVV}").FontSize(12);
                                 inner.Item().Text($"Zip: {model.Zip}").FontSize(12);
                             });
                        }

                        col.Item().PaddingVertical(8)
                            .LineHorizontal(1)
                            .LineColor(Colors.Grey.Lighten2);


                        //**********
                        col.Item().Row(row =>
                        {

                            row.RelativeItem().AlignCenter().PaddingTop(100).Text(text =>
                            {
                                //customer section
                                text.Span("Customer Electronic Signature").SemiBold();
                            });
                        });

                        col.Item().Row(row =>
                        {
                            // date & email
                            row.RelativeItem().Text(text =>
                            {
                                text.Span("Signed At:");
                                text.Span($"    {model.SignedAtUtc.ToString()}    ").Underline();
                            });

                            row.RelativeItem().Text(text =>
                            {
                                text.Span("Email Address:");
                                text.Span($"    {model.CustomerEmail}    ").Underline();
                            });

                        });

                        col.Item().Row(row =>
                        {
                            //number and typed signature
                            row.RelativeItem().Text(text =>
                            {
                                text.Span("Phone Number:");
                                text.Span($"  {model.CustomerPhoneNumber}    ").Underline();
                            });

                            row.RelativeItem().Text(text =>
                            {
                                text.Span("Typed Signature:");
                                text.Span($"  {model.Signature}    ").Underline();
                            });
                        });
                        col.Item().Row(row =>
                        {
                            row.RelativeItem().Text(text =>
                            {
                                text.Span("Billing Address:");
                                text.Span($"  {model.CustomerAddress}    ").Underline();
                            });

                            row.RelativeItem().Text(text =>
                            {
                                text.Span("Vessel:");
                                text.Span($"  {model.VesselLength} {model.VesselMakeModek}    ").Underline();
                            });

                        });

                        //*************
                        //col.Item().Row(row =>
                        //{

                        //    row.RelativeItem().AlignCenter().Text(text =>
                        //    {
                        //        //Captain section
                        //        text.Span("TowBoatUS Captain").SemiBold();
                        //    });
                        //});

                        //col.Item().Row(row =>
                        //{

                        //    row.RelativeItem().Text(text =>
                        //    {
                        //        text.Span("Captain Name");
                        //        text.Span($"    {model.CaptainName}    ").Underline();
                        //    });

                        //    row.RelativeItem().Text(text =>
                        //    {
                        //        text.Span("Captain Typed Signature");
                        //        text.Span($"    {model.CaptainName}    ").Underline();
                        //    });

                        //});
                    });
                });
            });



            var bytes = document.GeneratePdf();

            _pdfStore[model.Token] = bytes;
            return bytes;
        }

        public byte[] BuildAuditTrail(DispatchSignedPdfModel model)
        {
            var document = Document.Create(container =>
            {
                
                if (model.AuditEntry is not null)
                {
                    container.Page(page =>
                    {
                        page.Margin(30);
                        page.DefaultTextStyle(x => x.FontSize(12));

                        page.Header()
                            .Text("Audit Trail Addendum")
                            .SemiBold()
                            .AlignCenter()
                            .FontSize(16);

                        page.Content().Column(col =>
                        {
                            col.Spacing(6);
                            ///document-sign/{Token}
                            col.Item().Text($"Token: {model.AuditEntry.Token}");
                            col.Item().Text($"URL Link: https://towboatustb.com/document-sign/{model.AuditEntry.Token}");
                            col.Item().Text($"Link Created At: {model.SentAt}");
                            col.Item().Text($"Link Expires At: {model.ExpiresAt}");
                            col.Item().Text($"Customer Name: {model.AuditEntry.CustomerName}");
                            col.Item().Text($"Customer Email: {model.AuditEntry.CustomerEmail}");
                            col.Item().Text($"Form Name: {model.AuditEntry.FormName}");
                            col.Item().Text($"Signed At: {model.AuditEntry.SignedAtUtc.ToLocalTime().ToString("g")}");
                            col.Item().Text($"IP Address: {model.AuditEntry.IpAddress}");
                            col.Item().Text($"User Agent: {model.AuditEntry.UserAgent}");
                            //col.Item().Text($"Document Version: {model.AuditEntry.DocumentVersion}");
                            //col.Item().Text($"Document Hash (SHA-256): {model.AuditEntry.DocumentHashSha256}");
                            col.Item().Text($"Consent Checked: {model.AuditEntry.ConsentChecked}");

                            col.Item().PaddingTop(8).Text("Consent Text:").SemiBold();
                            col.Item().Text(model.AuditEntry.ConsentText);

                            col.Item().Text($"Signature Text: {model.AuditEntry.SignatureText}");
                            col.Item().Text($"Signed PDF Hash (SHA-256): {model.AuditEntry.SignedPdfHashSha256}");
                        });
                    });
                }
            });
            var bytes = document.GeneratePdf();

            _pdfStore[model.Token] = bytes;
            return bytes;
        }

        public byte[] BuildStandardFormSalvageContractPdf(DispatchSignedPdfModel model)
        {
            var document = Document.Create(container =>
            {
                
                static void UnderlineField(IContainer c, string? value, float minWidth = 80)
                {
                    c.MinWidth(minWidth)
                     .BorderBottom(1)
                     .BorderColor(Colors.Black)
                     .PaddingBottom(2)
                     .PaddingLeft(3)
                     .PaddingRight(3)
                     .AlignMiddle()
                     .AlignCenter()
                     .Text(string.IsNullOrWhiteSpace(value) ? " " : value);
                }

                static void HeaderBlock(IContainer container)
                {
                    container.AlignCenter().PaddingTop(-9).Column(col =>
                    {
                        col.Spacing(2);

                        col.Item().Text("AMERICAN MARINE SERVICES, LLC").AlignCenter().SemiBold().FontSize(12);
                        col.Item().Text("STANDARD FORM SALVAGE CONTRACT").AlignCenter().SemiBold().FontSize(12);
                        col.Item().Text("13621 Walsinham Road, Largo FL 33774").AlignCenter().SemiBold().FontSize(11);
                        col.Item().Text("(727) 347-3532").AlignCenter().SemiBold().FontSize(11);
                    });
                }

                static void RatesTable(IContainer container)
                {
                    // This table mirrors the 4-column “role / rate” layout shown in the contract (two pairs per row). :contentReference[oaicite:0]{index=0}
                    container.Table(table =>
                    {
                        table.ColumnsDefinition(cols =>
                        {
                            cols.RelativeColumn(1.35f);
                            cols.RelativeColumn(0.75f);
                            cols.RelativeColumn(1.35f);
                            cols.RelativeColumn(0.75f);
                        });

                        void Row(string leftLabel, string leftRate, string rightLabel, string rightRate)
                        {
                            table.Cell().PaddingVertical(1).Text(leftLabel);
                            table.Cell().PaddingVertical(1).Text(leftRate);
                            table.Cell().PaddingVertical(1).Text(rightLabel);
                            table.Cell().PaddingVertical(1).Text(rightRate);
                        }

                        Row("Salvage Master", "$ 325/hr", "Dive Master", "$ 275/hr");
                        Row("USCG Captain", "$ 130/hr", "Salvor", "$ 115/hr");
                        Row("Salvage Vessel", "$ 375/hr", "Electric Pump", "$ 75/hr");
                        Row("Salvage Vehicle", "$ 400/day", "2200lb Air Bag", "$ 400/day");
                        Row("Salvage Diver", "$175/hr", "Skilled Labor", "$ 90/hr");
                        Row("Gas Engine Pump", "$ 125/hr", "4400lb Air Bag", "$ 600/day");
                        Row("Operations Center", "$500/day", "Service Vehicle", "$ 250/day");
                        Row("Reconstitute/Repack", "$ 1000/job", "Boat Trailer", "$ 350/day");
                    });
                }


                // =========================
                // PAGE 1 (Front) :contentReference[oaicite:1]{index=1}
                // =========================
                container.Page(page =>
                {
                    page.Size(PageSizes.Letter);
                    page.Margin(26);
                    page.DefaultTextStyle(x => x.FontSize(11));

                    page.Header().ShowOnce().Element(HeaderBlock);

                    page.Content().StopPaging().Column(col =>
                    {
                        col.Spacing(1);

                        // "It is hereby agreed this ___ day of _________, 20___, at ______ hours at ______________________ (location)"
                        col.Item().Text(text =>
                        {
                            text.Span("It is hereby agreed on ");
                            text.Element(e => UnderlineField(e, $"{model.SentAt}", 10));
                            text.Span(" at ");
                            text.Element(e => UnderlineField(e, $"{model.GPS}", 300));
                        });

                        col.Item().PaddingLeft(350).Text("(location)").FontSize(9).FontColor(Colors.Grey.Darken2);

                        // "by and between: _____________________________ for the Vessel named _____________________ (Owner/Captain/Agent)"
                        col.Item().Text(text =>
                        {
                            text.Span("by and between: ");
                            text.Element(e => UnderlineField(e, $"{model.CustomerName}", 120));
                            text.Span(" for the Vessel named ");
                            text.Element(e => UnderlineField(e, $"{model.VesselName}", 120));
                        });

                        col.Item().PaddingLeft(100).Text("(Owner/ Captain/Agent)").FontSize(9).FontColor(Colors.Grey.Darken2);

                        // "described as a_____Ft, ______________________________________and insured by:  _________________"
                        col.Item().Text(text =>
                        {
                            text.Span("described as a ");
                            text.Element(e => UnderlineField(e, $"{model.VesselLength}", 20));
                            text.Span(" Ft, ");
                            text.Element(e => UnderlineField(e, $"{model.VesselMakeModek}", 200));
                            text.Span(" and insured by: ");
                            text.Element(e => UnderlineField(e, $"{model.Underwriter}", 150));
                        });

                        col.Item().PaddingLeft(70).Text("(length)            (year – manufacturer – FL or Doc numbers)                                                       (\"Underwriter\")")
                            .FontSize(9).FontColor(Colors.Grey.Darken2);

                        col.Item().Text("and American Marine Services, LLC, to salvage the vessel under these terms and conditions:");

                        // Clauses 1-4
                        col.Item().Text(text =>
                        {
                            text.Span("1. Salvor agrees to render assistance to and endeavor to save said vessel and its property and deliver her afloat or ashore at ");
                            text.Element(e => UnderlineField(e, $"{model.Delivery}", 230));
                            text.Span(" marina or port as mutually agreed, or to nearest safe port if unspecified herein, as soon as practicable.");
                        });

                        col.Item().Text("2. Salvor shall have the requisite control of the subject yacht and be entitled without expense to the reasonable use of the yacht and its gear in the performance of recovery or salvage operations.  American Marine Services, LLC is not responsible for damages to the vessel that may occur as part of the salvage operation.");

                        col.Item().Text("3. Said salvage operation by the Salvor shall terminate upon delivery of said yacht as designated herein. Owner and Underwriter shall be responsible for all risk of loss after thereafter and for any storage charges or charges for subsequent towing to another port or marina.");

                        col.Item().Text("4. In the event it becomes necessary to obtain legal counsel to enforce the tower’s/salvor’s rights under this contract, whether suit is filed or not, the tower/salvor shall be entitled to recover its costs and expenses, including but not limited to reasonable attorneys’ fees, incurred in the enforcement and/or collection of this contract.  Salvage services are undertaken under one of the following agreements by Owner and Salvor:");

                        // (a) Pure Salvage + initials
                        col.Item().PaddingLeft(14).PaddingTop(5).Text("(a)  Pure Salvage - Compensation, including special compensation, to be determined under ARTICLES 13 and 14, SALCON 89, and U.S. Admiralty Law.");

                        col.Item().PaddingLeft(14).Row(r =>
                        {
                            r.AutoItem().Text("Signature");
                            r.Spacing(8);
                            // removing model.CaptainSignature + model.PriceSignature
                            //TO-DO: PROPERLY ROUTE WHEN pricing model a) or c) are selected!!!
                            r.AutoItem().Element(e => UnderlineField(e, $"  ", 80));
                            r.AutoItem().Text("/");
                            r.AutoItem().Element(e => UnderlineField(e, $"  ", 80));
                        });

                        col.Item().PaddingLeft(86).Text("     salvor                          owner/agent").FontSize(9).FontColor(Colors.Grey.Darken2);

                        // (b) Fixed Price + initials
                        col.Item().PaddingLeft(14).Text(text =>
                        {
                            text.Span("(b)  Fixed Price (No Cure/No Pay) of $ ");
                            text.Element(e => UnderlineField(e, $"{model.Quote}", 140));
                            text.Span(" afloat at point of service. Delivery ashore or to a marina or boat ramp is on a time and material basis using the rates listed in paragraph 4 (c) below.");
                        });

                        col.Item().PaddingLeft(14).Row(r =>
                        {
                            r.AutoItem().Text("Signature");
                            r.Spacing(8);

                            r.AutoItem().Element(e => UnderlineField(e, $"{model.CaptainSignature}", 80));
                            r.AutoItem().Text("/");
                            r.AutoItem().Element(e => UnderlineField(e, $"{model.PriceSignature}", 80));
                        });

                        col.Item().PaddingLeft(86).Text("     salvor                          owner/agent").FontSize(9).FontColor(Colors.Grey.Darken2);

                        // (c) Time and Material + rates table + recovery fee + initials
                        col.Item().PaddingLeft(14).Text(" (c) Time and Material - Payable at all events; hourly rates are billed portal to portal.  Hourly rates are uplifted by 100% for hazardous/explosive conditions, including Small Craft Advisory conditions.  Labor rates increase by 50% from 1800 to 0800 and on weekends. Outside Contractors, Equipment and Supplies are invoiced at cost plus 20%. Environmental services invoiced separately at cost plus 20%.");

                        col.Item().PaddingLeft(14).PaddingTop(6).Element(RatesTable);

                        col.Item().PaddingLeft(14).PaddingTop(6).Text(text =>
                        {
                            text.Span("Plus, Recovery/Ungrounding fee of $");
                            text.Element(e => UnderlineField(e, $" ", 120));
                            text.Span(" per foot times length of vessel when job is complete.");
                        });

                        col.Item().PaddingLeft(14).Row(r =>
                        {
                            r.AutoItem().Text("Signature");
                            r.Spacing(8);
                            // removing model.CaptainSignature + model.PriceSignature
                            //TO-DO: PROPERLY ROUTE WHEN pricing model a) or c) are selected!!!
                            r.AutoItem().Element(e => UnderlineField(e, $"  ", 80));
                            r.AutoItem().Text("/");
                            r.AutoItem().Element(e => UnderlineField(e, $"  ", 80));
                        });

                        col.Item().PaddingLeft(86).Text("     salvor                          owner/agent").FontSize(9).FontColor(Colors.Grey.Darken2);

                        //col.Item().AlignRight().Text("Rev Feb 15, 2024").FontSize(9).FontColor(Colors.Grey.Darken2);
                    });
                });

                // =========================
                // PAGE 2 (Back) :contentReference[oaicite:2]{index=2}
                // =========================
                container.Page(page =>
                {
                    page.Size(PageSizes.Letter);
                    page.Margin(26);
                    page.DefaultTextStyle(x => x.FontSize(11));

                    page.Content().StopPaging().Column(col =>
                    {
                        col.Spacing(1);

                        col.Item().Text("5. Compensation to Salvor for the services performed hereunder shall be in accordance with a billing and any supportive analysis of the salvage operation to be presented to Owner and/or Underwriter's agents upon completion of salvage. Billing to be calculated on the basis specified in No. 4.  No agreement on price or its reasonableness has been made at the scene unless agreed to in writing.");
                        col.Item().Text("6. Salvor shall be entitled to a reasonable allowance for prevention or minimization of environmental damage in accordance with Articles 13 & 14 of the 1989 International Convention on Salvage, as well as for clean up or wreck removal billed on a time & material basis should the vessel be deemed a constructive total loss.  Owner of vessel is solely responsible for all environmental damages that may result from the loss and subsequent salvage of the vessel.");
                        col.Item().Text("7. Payment is due promptly upon presentation of Salvor's bill. In the event the Owner or Captain or the vessel’s insurer fails to make prompt payment, Salvor may claim a lien on the vessel for the value of the services rendered.  Interest shall accrue at the rate of one and one-half (1.5%) percent per month or the maximum legal rate allowed by law on any unpaid balance from 30 days after completion of the salvage services and presentation of the bill or as may be determined in accordance with the findings of any Court or Arbitration Award.");
                        col.Item().Text("8. In the event of any dispute regarding this salvage or concerning the reasonableness of any fees or charges due hereunder, all parties agree to binding arbitration pursuant to The Boat Owners' Association of the United States' Salvage Arbitration Plan.  If either party must compel binding arbitration, all parties consent to the jurisdiction and venue of the Federal Court Middle District of Florida upon service of process made in accordance with the statutes of the United States.  All parties waive any and all rights to object to in personam jurisdiction in said described forum for the purpose of litigation commenced to compel arbitration.");
                        col.Item().Text("9. It is understood that services performed hereunder are governed by the Admiralty and Maritime Jurisdiction of the Federal Courts and create a maritime lien against the yacht or its posted security. Salvor's lien shall be preserved until payment. Salvor agrees in lieu of arrest or attachment to accept from the yacht's Underwriter, a Letter of Undertaking for an amount equal to one and one- half (1.5) times the presented billing with a copy of the insurance policy and coverage information. If the yacht is uninsured or its Underwriter cannot provide a Letter of Undertaking, Salvor may demand the posting of a Surety Bond with its designated Escrow Agent in an amount equal to 1.5 times the Salvor's bill within 60 days of receipt of the salvor’s bill. Salvor may satisfy collection of fees or charges hereunder by recourse to any security posted and shall be entitled to any costs incurred in collection of payments due hereunder including reasonable attorney’s fees subject to the findings of any arbitration or court.");
                        col.Item().Text("10. Should Salvor be required to compel arbitration by court action, Salvor shall be entitled to recover from the Owner, Captain and/or the vessel’s Insurer, all costs and fees incurred by Salvor, including attorney’s fees.");
                        col.Item().Text("11. Salvor hereby warrants that it is acting on its own behalf and on behalf of any subcontractors retained by Salvor to perform services in the recovery or delivery of the yacht. Salvor shall be responsible for any such subcontractors' compensation.");
                        col.Item().Text("12. In the event the Salvor has already rendered salvage services to the described yacht prior to execution of this contract, the provisions of this contract shall apply to such salvage services.");

                        col.Item().Text(text =>
                        {
                            text.Span("13. I hereby acknowledge and agree to the authorization for Assignment of Insurance Benefits and Direct Payment Authorization to American Marine Services, LLC for all services rendered under this salvage agreement contract.  ");
                            text.Element(e => UnderlineField(e, $"{model.Signature}", 60));
                            text.Span("(Signature) I make this assignment and authorization in consideration for the salvage serveries rendered.");
                        });

                        col.Item().Text("I acknowledge that I understand the obligations set forth in this agreement.");
                        col.Item().Text(text =>
                        {
                            text.Span("Claim #:");
                            text.Element(e => UnderlineField(e, $"{model.ClaimNumber}", 120));
                        });

                        col.Item().Height(2);

                        // SIGNATURES
                        col.Item().Row(row =>
                        {
                            row.RelativeItem().Column(left =>
                            {
                                left.Spacing(4);
                                left.Item().Text("SIGNED: ");
                                left.Item().Element(e => UnderlineField(e, $"{model.Signature}", 200));
                                left.Item().PaddingLeft(100).Text("(Owner/Captain or Agent Signature)").FontSize(9).FontColor(Colors.Grey.Darken2);
                            });

                            row.RelativeItem().Column(right =>
                            {
                                right.Spacing(4);
                                right.Item().Text("SIGNED: ");
                                right.Item().Element(e => UnderlineField(e, $"{model.CaptainSignature}", 260));
                                right.Item().PaddingLeft(60).Text("(Agent Signature, American Marine Services, LLC)").FontSize(9).FontColor(Colors.Grey.Darken2);
                            });
                        });

                        col.Item().Height(2);

                        col.Item().Row(row =>
                        {
                            row.RelativeItem().Column(c =>
                            {
                                c.Spacing(4);
                                c.Item().Element(e => UnderlineField(e, $"{model.CustomerName}", 200));
                                c.Item().PaddingLeft(100).Text("(Customer Name)").FontSize(9).FontColor(Colors.Grey.Darken2);
                            });

                            row.RelativeItem().Column(c =>
                            {
                                c.Spacing(4);
                                c.Item().Element(e => UnderlineField(e, $"{model.CustomerAddress}", 200));
                                c.Item().PaddingLeft(120).Text("(Customer Address)").FontSize(9).FontColor(Colors.Grey.Darken2);
                            });
                        });
                        col.Item().Height(2);

                        col.Item().Row(row =>
                        {
                            row.RelativeItem().Column(c =>
                            {
                                c.Spacing(4);
                                c.Item().Element(e => UnderlineField(e, $"{model.CustomerPhoneNumber}", 200));
                                c.Item().PaddingLeft(100).Text("(Customer Phone)").FontSize(9).FontColor(Colors.Grey.Darken2);
                            });

                            row.RelativeItem().Column(c =>
                            {
                                c.Spacing(4);
                                c.Item().Element(e => UnderlineField(e, $"{model.SignedAtUtc.ToLocalTime().ToString("g")}", 200));
                                c.Item().PaddingLeft(120).Text("(Signed at)").FontSize(9).FontColor(Colors.Grey.Darken2);
                            });
                        });
                    });
                });

            });
            var bytes = document.GeneratePdf();

            _pdfStore[model.Token] = bytes;
            return bytes;
        }

        public bool TryGetPdf(string token, out byte[]? bytes)
        {
            return _pdfStore.TryGetValue(token, out bytes);
        }
    }

    public sealed class DispatchSignedPdfModel
    {
        public string Token { get; set; } = "";
        public string SentAt { get; set; } = "";
        public string ExpiresAt { get; set; } = "";
        public string CustomerName { get; set; } = "";
        public string CustomerEmail { get; set; } = "";
        public string CustomerPhoneNumber { get; set; } = "";
        public string FormName { get; set; } = "";
        public string BoatRegistration { get; set; } = "";
        public string VesselName { get; set; } = "";
        public string Signature { get; set; } = "";
        public string CaptainName { get; set; } = "";
        public string CaptainSignature { get; set; } = "";
        public string VesselLength { get; set; } = "";
        public string VesselMakeModek { get; set; } = "";
        public string GPS { get; set; } = "";
        public string UnaccompaniedOrigin { get; set; } = "";
        public string UnaccompaniedDestination { get; set; } = "";
        public string Underwriter { get; set; } = "";
        public string Delivery { get; set; } = "";
        public string Quote { get; set; } = "";
        public string ClaimNumber { get; set; } = "";
        public string CustomerAddress { get; set; } = "";
        public string PriceSignature { get; set; } = "";
        public string ConsentText { get; set; } = "";
        public string DocumentContent { get; set; } = "";
        public string DocumentHashSha256 { get; set; } = "";
        public string DocumentVersion { get; set; } = "";
        public DateTime SignedAtUtc { get; set; }

        public DispatchAuditEntry? AuditEntry { get; set; }

        public string CardHolderName { get; set; } = "";
        public string CardNumber { get; set; } = "";
        public string Expiration { get; set; } = "";
        public string CVV { get; set; } = "";
        public string CardType { get; set; } = "";
        public string Zip { get; set; } = "";
        public string AuthorizedAmount { get; set; } = "";
    }

    public sealed class SalvageContractPdfModel
    {
        // Top section
        public string DayNumber { get; set; } = "";          // e.g. "15"
        public string MonthName { get; set; } = "";          // e.g. "February"
        public string YearNumber { get; set; } = "";         // e.g. "2026"
        public string Hours { get; set; } = "";              // e.g. "1430"
        public string Location { get; set; } = "";           // e.g. "John's Pass, FL"

        public string OwnerCaptainAgentName { get; set; } = "";
        public string VesselName { get; set; } = "";

        // Described as: ___Ft, ______________________________________ and insured by: __________
        public string VesselLengthFt { get; set; } = "";
        public string VesselYearManufacturerNumbers { get; set; } = ""; // "year – manufacturer – FL or Doc numbers"
        public string Underwriter { get; set; } = "";

        // Clause 1 destination
        public string DeliveryMarinaOrPort { get; set; } = "";

        // Pricing selection initials
        public string InitialsSalvor_PureSalvage { get; set; } = "";
        public string InitialsOwner_PureSalvage { get; set; } = "";

        public string FixedPriceAmount { get; set; } = ""; // "$ _____________"
        public string InitialsSalvor_Fixed { get; set; } = "";
        public string InitialsOwner_Fixed { get; set; } = "";

        public string InitialsSalvor_TimeMaterial { get; set; } = "";
        public string InitialsOwner_TimeMaterial { get; set; } = "";

        public string RecoveryUngroundingFeePerFoot { get; set; } = ""; // "$______ per foot"
        public string InitialsSalvor_RecoveryFee { get; set; } = "";
        public string InitialsOwner_RecoveryFee { get; set; } = "";

        // Page 2 / signatures
        public string OwnerCaptainAgentSignature { get; set; } = "";
        public string SalvorAgentSignature { get; set; } = "";

        public string PrintedName { get; set; } = "";
        public string AddressLine { get; set; } = ""; // if you want to override; otherwise you can keep the company address static

        public string OwnerPhone { get; set; } = "";

        public string AssignmentInitial { get; set; } = ""; // clause 13 initial
    }
}