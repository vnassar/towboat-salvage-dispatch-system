namespace TowBoatSalvageWebApp.Models
{
    public class DocumentSignatureRequest
    {
        public long Id { get; set; }
        public string Token { get; set; } = "";
        public string CaptainEmail { get; set; } = "";
        public string CaptainName { get; set; } = "";

        public string CustomerName { get; set; } = "";
        public string CustomerEmail { get; set; } = "";
        public string CustomerPhone { get; set; } = "";
        public string FormName { get; set; } = "";
        public string GPS { get; set; } = "";

        public DocumentSignatureStatus Status { get; set; } = DocumentSignatureStatus.Pending;

        public DateTime SentAtUtc { get; set; } 
        public DateTime ExpiresAtUtc { get; set; }

        public DateTime? SignedAtUtc { get; set; }

        public DateTime? EmailAcceptedAtUtc { get; set; }
        public DateTime? EmailDeliveredAtUtc { get; set; }
        public DateTime? EmailOpenedAtUtc { get; set; }
        public DateTime? EmailFailedAtUtc { get; set; }
        public string? EmailLastEvent { get; set; }
        public string? EmailFailureReason { get; set; }

        public string? SignedByName { get; set; }
        public string? VesselLength { get; set; }
        public string? VesselMakeModel { get; set; }
        public string? VesselName { get; set; }
        public string? Underwriter { get; set; }
        public string? Delivery { get; set; } 
        public string? Quote { get; set; } 
        public string? ClaimNumber { get; set; } 
        public string? SalvagePricingModel { get; set; } 
        public string? Signature { get; set; }
        public string? CustomerAddress { get; set; }
        public string? UnaccompaniedOrigin { get; set; }
        public string? UnaccompaniedDestination { get; set; }
        public byte[]? SignedPdf { get; set; }
        public byte[]? AuditTrail { get; set; }
        public CreditCard? CreditCard { get; set; } = new();
    }

    public enum DocumentSignatureStatus
    {
        Pending = 0,
        Signed = 1,
        Expired = 2
    }

    public class CreditCard()
    {
        public long Id { get; set; }
        public string? CardHolderName { get; set; }
        public string?  CardNumber { get; set; }
        public string? Expiration { get; set; }
        public string? CVV { get; set; }
        public string? AuthorizedAmount { get; set; }
    }

}
