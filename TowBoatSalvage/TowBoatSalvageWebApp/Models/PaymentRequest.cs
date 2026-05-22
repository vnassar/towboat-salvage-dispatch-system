namespace TowBoatSalvageWebApp.Models
{
    /// <summary>
    /// Tracks a one-time Stripe payment request sent by a captain to a customer
    /// </summary>
    public class PaymentRequest
    {
        public long Id { get; set; }

        public string CaptainEmail { get; set; } = "";
        public string CaptainName { get; set; } = "";

        public string CustomerName { get; set; } = "";
        public string CustomerEmail { get; set; } = "";
        public string? CustomerPhone { get; set; }

        public string Description { get; set; } = "TowBoatUS Towing Service";
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "usd";

        public string StripePaymentLinkId { get; set; } = "";
        public string StripePaymentLinkUrl { get; set; } = "";
        public string? StripeSessionId { get; set; }
        public string? StripeReceiptUrl { get; set; }

        // Unique token For Mailgun
        public string EmailTrackingToken { get; set; } = "";

        public PaymentRequestStatus Status { get; set; } = PaymentRequestStatus.Pending;
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
        public DateTime? PaidAtUtc { get; set; }

        public DateTime? EmailAcceptedAtUtc { get; set; }
        public DateTime? EmailDeliveredAtUtc { get; set; }
        public DateTime? EmailOpenedAtUtc { get; set; }
        public DateTime? EmailFailedAtUtc { get; set; }
        public string? EmailLastEvent { get; set; }
        public string? EmailFailureReason { get; set; }
    }

    public enum PaymentRequestStatus
    {
        Pending,
        Paid,
        Expired,
        Cancelled
    }
}
