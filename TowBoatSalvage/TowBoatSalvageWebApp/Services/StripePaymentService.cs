using Microsoft.EntityFrameworkCore;
using Stripe;
using TowBoatSalvageWebApp.Data;
using TowBoatSalvageWebApp.Models;

namespace TowBoatSalvageWebApp.Services
{
    /// <summary>
    /// Creates Stripe Payment Links and manages PaymentRequest records.
    /// </summary>
    public class StripePaymentService
    {
        private readonly IDbContextFactory<SalvageDbContext> _dbFactory;
        private readonly IConfiguration _config;
        private readonly ILogger<StripePaymentService> _logger;

        public StripePaymentService(
            IDbContextFactory<SalvageDbContext> dbFactory,
            IConfiguration config,
            ILogger<StripePaymentService> logger)
        {
            _dbFactory = dbFactory;
            _config = config;
            _logger = logger;
        }


        public async Task<PaymentRequest> CreatePaymentRequestAsync(
            string captainEmail,
            string captainName,
            string customerName,
            string customerEmail,
            string? customerPhone,
            decimal amount,
            string description = "TowBoatUS Towing Service",
            string currency = "usd")
        {

            var priceOptions = new PriceCreateOptions
            {
                UnitAmount = (long)(amount * 100),
                Currency = currency,
                ProductData = new PriceProductDataOptions
                {
                    Name = description,
                },
            };

            var priceService = new PriceService();
            var price = await priceService.CreateAsync(priceOptions);


            var thankYouUrl = _config["Stripe:ThankYouUrl"] ?? "https://towboatustb.com/thank-you";

            var linkOptions = new PaymentLinkCreateOptions
            {
                LineItems =
                [
                    new PaymentLinkLineItemOptions
                {
                    Price = price.Id,
                    Quantity = 1,
                }
                ],
                AfterCompletion = new PaymentLinkAfterCompletionOptions
                {
                    Type = "redirect",
                    Redirect = new PaymentLinkAfterCompletionRedirectOptions
                    {
                        Url = thankYouUrl,
                    },
                },
                Metadata = new Dictionary<string, string>
                {
                    ["customer_email"] = customerEmail,
                    ["captain_email"] = captainEmail,
                },
            };

            var linkService = new PaymentLinkService();
            var paymentLink = await linkService.CreateAsync(linkOptions);

            await using var db = await _dbFactory.CreateDbContextAsync();

            var paymentRequest = new PaymentRequest
            {
                CaptainEmail = captainEmail,
                CaptainName = captainName,
                CustomerName = customerName,
                CustomerEmail = customerEmail,
                CustomerPhone = customerPhone,
                Amount = amount,
                Currency = currency,
                Description = description,
                StripePaymentLinkId = paymentLink.Id,
                StripePaymentLinkUrl = paymentLink.Url,
                Status = PaymentRequestStatus.Pending,
                CreatedAtUtc = DateTime.UtcNow,
                //prefixed with "pay_" so the webhook can distinguish payment tokens from document tokens
                EmailTrackingToken = $"pay_{Guid.NewGuid():N}"
            };

            db.PaymentRequests.Add(paymentRequest);
            await db.SaveChangesAsync();

            _logger.LogInformation(
                "Payment request created. Captain={Captain}, Customer={Customer}, Amount={Amount}, LinkId={LinkId}",
                captainEmail, customerEmail, amount, paymentLink.Id);

            return paymentRequest;
        }

        /// <summary>
        /// Called by the Stripe webhook when a checkout session completes.
        /// Marks the matching PaymentRequest as Paid.
        /// </summary>
        public async Task MarkAsPaidAsync(string paymentLinkId, string sessionId, string? paymentIntentId)
        {
            await using var db = await _dbFactory.CreateDbContextAsync();

            var request = await db.PaymentRequests
                .FirstOrDefaultAsync(p => p.StripePaymentLinkId == paymentLinkId);

            if (request is null)
            {
                _logger.LogWarning("No PaymentRequest found for Stripe link {LinkId}", paymentLinkId);
                return;
            }

            request.Status = PaymentRequestStatus.Paid;
            request.PaidAtUtc = DateTime.UtcNow;
            request.StripeSessionId = sessionId;

            // get payment receipt to display in UI
            if (!string.IsNullOrEmpty(paymentIntentId))
            {
                try
                {
                    var piService = new Stripe.PaymentIntentService();
                    var paymentIntent = await piService.GetAsync(paymentIntentId);

                    if (!string.IsNullOrEmpty(paymentIntent.LatestChargeId))
                    {
                        var chargeService = new Stripe.ChargeService();
                        var charge = await chargeService.GetAsync(paymentIntent.LatestChargeId);

                        request.StripeReceiptUrl = charge.ReceiptUrl;

                        _logger.LogInformation("Receipt URL saved for PaymentRequest {Id}: {Url}", request.Id, charge.ReceiptUrl);
                    }
                }
                catch (Stripe.StripeException ex)
                {
                    _logger.LogWarning(ex, "Could not fetch receipt URL for PaymentIntent {PaymentIntentId}", paymentIntentId);
                }
            }

            await db.SaveChangesAsync();

            _logger.LogInformation("PaymentRequest {Id} marked as Paid. SessionId={SessionId}", request.Id, sessionId);
        }

        /// <summary>
        /// Load payment requests for the History tab.
        /// Admins see all; captains see only their own.
        /// </summary>
        public async Task<List<PaymentRequest>> GetRequestsAsync(string? captainEmail, bool isAdmin)
        {
            await using var db = await _dbFactory.CreateDbContextAsync();

            var query = db.PaymentRequests.AsQueryable();

            if (!isAdmin && !string.IsNullOrWhiteSpace(captainEmail))
            {
                query = query.Where(p => p.CaptainEmail == captainEmail);
            }

            return await query
                .AsNoTracking()
                .OrderByDescending(p => p.CreatedAtUtc)
                .ToListAsync();
        }

        public async Task DeletePaymentEntryAsync(long id)
        {
            await using var db = await _dbFactory.CreateDbContextAsync();

            var paymentEntry = await db.PaymentRequests.FindAsync(id);
            if (paymentEntry is not null)
            {
                db.PaymentRequests.Remove(paymentEntry);
                await db.SaveChangesAsync();
            }
        }
    }
}
