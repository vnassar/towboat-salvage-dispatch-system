namespace TowBoatSalvageWebApp.Models
{
    public class WorkOrder
    {
        public int Id { get; set; }
        public string VesselName { get; set; } = "";
        public DateTime? RequestDateDisplay { get; set; } 
        public string Engine1Hours { get; set; } = "";
        public string Engine2Hours { get; set; } = "";
        public List<string> ReportedIssues { get; set; } = new();

        //multiple corrections per issue
        public Dictionary<int, List<IssueCorrection>> IssueCorrectionThreads { get; set; } = new();

        public List<string> IssueCorrections { get; set; } = new();
        public List<string> IssueCorrectionsBy { get; set; } = new();
        public bool IsAddingCorrection { get; set; }
        public string CorrectionNotes { get; set; } = "";
        public string FromCrewMember { get; set; } = "";
        public bool IsResolved { get; set; }
        public bool bHasBeenDownloaded { get; set; } 
    }
}
