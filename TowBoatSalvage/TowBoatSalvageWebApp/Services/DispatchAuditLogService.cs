using System.Collections.Concurrent;

namespace TowBoatSalvageWebApp.Services
{
    public sealed class DispatchAuditLogService
    {
        private readonly ConcurrentQueue<DispatchAuditEntry> _entries = new();

        public void Add(DispatchAuditEntry entry) => _entries.Enqueue(entry);

        public IReadOnlyCollection<DispatchAuditEntry> GetAll() => _entries.ToArray();


    }

    public sealed class DispatchAuditEntry
    {
        public string Token { get; set; } = "";
        public string CustomerName { get; set; } = "";
        public string CustomerEmail { get; set; } = "";
        public string FormName { get;set ;} = "";
        public string ConsentText {get;set;} = "";
        public DateTime SignedAtUtc {get;set;}
        public string IpAddress {get;set;   } = "";
        public string UserAgent {get;set;} = "";

        public string DocumentVersion {get;set;} = "";
        public string DocumentHashSha256 {get;set;} = "";
        public bool ConsentChecked {get;set;}

        public string SignatureText { get; set; } = "";
        public string SignedPdfHashSha256 { get; set; } = "";
    }
}