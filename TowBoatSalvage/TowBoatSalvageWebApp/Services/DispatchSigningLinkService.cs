using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using TowBoatSalvageWebApp.Data;
using TowBoatSalvageWebApp.Models;

namespace TowBoatSalvageWebApp.Services
{
    public sealed class DispatchSigningLinkService
    {
        private readonly SalvageDbContext _db;

        public DispatchSigningLinkService(SalvageDbContext db)
        {
            _db = db;
        }

        private readonly ConcurrentDictionary<string, SigningLinkInfo> __links = new();
        
        public SigningLinkInfo CreateLink(string customerName, string customerEmail, string customerPhone, string formName, TimeSpan ttl)
        {
            var token = Guid.NewGuid().ToString("N");
            var now = DateTime.UtcNow;

            var info = new SigningLinkInfo
            {
                Token = token,
                CustomerName = customerName,
                CustomerEmail = customerEmail,
                CustomerPhone = customerPhone,
                FormName = formName,
                CreatedAtUtc = now,
                ExpiresAtUtc = now.Add(ttl),
                Used = false
            };

            __links[token] = info;
            return info;
        }

        public bool TryGetValid(string token, out SigningLinkInfo? info)
        {
            info = null;
            if (!__links.TryGetValue(token, out var exisitng)) return false;

            if (exisitng.Used || exisitng.ExpiresAtUtc <= DateTime.UtcNow) return false;

            info = exisitng;
            return true;
        }

        public void MarkUsed(string token)
        {
            if (__links.TryGetValue(token, out var existing))
            {
                existing.Used = true;
            }
        }

        public void CleanupExpired()
        {
            var now = DateTime.UtcNow;
            foreach (var kvp in __links)
            {
                if (kvp.Value.ExpiresAtUtc <= now || kvp.Value.Used) __links.TryRemove(kvp.Key, out _);
            }
        }

        public SigningLinkInfo CreateLink(
            string customerName,
            string customerEmail,
            string customerPhone,
            string formName,
            string captainEmail,
            string captainName,
            string gps,
            TimeSpan ttl
        )
        {
            var token = Guid.NewGuid().ToString("N");
            var now = DateTime.UtcNow;

            var info = new SigningLinkInfo
            {
                Token = token,
                CustomerName = customerName,
                CustomerEmail = customerEmail,
                CustomerPhone = customerPhone,
                FormName = formName,
                CaptainEmail = captainEmail,
                CaptainName = captainName,
                GPS = gps,
                CreatedAtUtc = now,
                ExpiresAtUtc = now.Add(ttl),
                Used = false
            };

            __links[token] = info;
            return info;
        }

        public async Task<DocumentSignatureRequest> CreateRequestAsync(string customerName, string customerEmail, string customerPhone, string formName, string captainEmail, string captainName, string gps, TimeSpan ttl, string? unaccompaniedOrigin = null, string? unaccompaniedDestination = null, string? underwriter = null, string? delivery = null, string? quote = null, string? claimNumber = null, string? customerAddress = null, string? vesselLength = null, string? vesselmakeModel = null, string? vesselName = null,string? salvagePricingModel = null)
        {
            var token = Guid.NewGuid().ToString("N");
            var now = DateTime.UtcNow;

            var request = new DocumentSignatureRequest
            {
                Token = token,
                CustomerName = customerName,
                CustomerEmail = customerEmail,
                CustomerPhone = customerPhone,
                FormName = formName,
                CaptainEmail = captainEmail,
                CaptainName = captainName, // Signature and Name are equal, not using a separate signature string. 
                GPS = gps,
                SentAtUtc = now,
                ExpiresAtUtc = now.Add(ttl),
                Status = DocumentSignatureStatus.Pending,
                UnaccompaniedOrigin = unaccompaniedOrigin,
                UnaccompaniedDestination = unaccompaniedDestination,
                //for james standard salvage contract form 
                Underwriter = underwriter,
                Delivery = delivery,
                Quote = quote,
                ClaimNumber = claimNumber,
                CustomerAddress = customerAddress,
                VesselLength = vesselLength,
                VesselMakeModel = vesselmakeModel,
                VesselName = vesselName,
                SalvagePricingModel = salvagePricingModel
                
            };

            _db.DocumentSignatureRequests.Add(request);
            await _db.SaveChangesAsync();
            return request;
        }

        public async Task<DocumentSignatureRequest?> GetValidRequestAsync(string token)
        {
            var now = DateTime.Now;

            var request = await _db.DocumentSignatureRequests.FirstOrDefaultAsync(x => x.Token == token);

            if (request is null) return null;
            if (request.Status != DocumentSignatureStatus.Pending) return null;
            if (request.ExpiresAtUtc <= now) return null;

            return request;
        }

        public async Task<bool> MarkSignedAsync(string token, string signedByName, string customerPhone,string vesselLength, string vesselMakeModel, string signature, string gps,byte[] signedPdf, byte[] auditTrail)
        {
            var request = await _db.DocumentSignatureRequests.FirstOrDefaultAsync(x => x.Token == token);

            if (request is null) return false;

            request.Status = DocumentSignatureStatus.Signed;
            request.SignedAtUtc = DateTime.Now;
            request.SignedByName = signedByName;
            request.CustomerPhone = customerPhone;
            request.VesselLength = vesselLength;
            request.VesselMakeModel = vesselMakeModel;
            request.Signature = signature;
            request.GPS = gps;
            request.SignedPdf = signedPdf;
            request.AuditTrail = auditTrail;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> MarkCreditCardAuthorization(string token, string signedByName, string customerPhone, string vesselLength, string vesselMakeModel, string signature, string gps, byte[] signedPdf, byte[] auditTrail, CreditCard creditCard)
        {
            var request = await _db.DocumentSignatureRequests.FirstOrDefaultAsync(x => x.Token == token);

            if (request is null) return false;

            request.Status = DocumentSignatureStatus.Signed;
            request.SignedAtUtc = DateTime.Now;
            request.SignedByName = signedByName;
            request.CustomerPhone = customerPhone;
            request.VesselLength = vesselLength;
            request.VesselMakeModel = vesselMakeModel;
            request.Signature = signature;
            request.GPS = gps;
            request.SignedPdf = signedPdf;
            request.AuditTrail = auditTrail;
            request.CreditCard = creditCard;

            await _db.SaveChangesAsync();
            return true;
        }

        public sealed class SigningLinkInfo
        {
            public string Token { get; set; } = "";
            public string CaptainEmail {get;set;} = "";
            public string CaptainName {get;set;} = "";
            public string CustomerName { get; set; } = "";
            public string CustomerEmail { get; set; } = "";
            public string CustomerPhone { get; set; } = "";
            public string FormName { get; set; } = "";
            public string GPS { get; set; } = "";
            public string VesselName { get; set; } = "";
            public string VesselLength { get; set; } = "";
            public string VesselMakeModel { get; set; } = "";
            public DateTime CreatedAtUtc { get; set; }
            public DateTime ExpiresAtUtc { get; set; }
            public bool Used { get; set; }

        }
    }
}