using System;

namespace TowBoatSalvageWebApp.Models
{
    public class VesselInspection
    {
        public int Id {get;set;}
        public string BoatNumber { get; set; } = string.Empty;
        public string CompletedBy { get; set; } = string.Empty;
        public DateTime? DateOfInspection { get; set; } = DateTime.Now;
        public bool bHasBeenDownloaded { get;set;} = false;
        public bool bIsResolved {get;set;} = false;
        public List<ServiceDescriptionVesselInspection> ServiceDescriptions {get;set;} = new();
        public string Notes {get;set;} = string.Empty;
    }

    public class ServiceDescriptionVesselInspection
    {
        public int Id {get;set;}
        public bool bServiceCompleted {get;set;} = false;
        public string Description {get;set;} = string.Empty;
        
        //only some items have this so null for most
        public DateTime? DateForThisItem {get;set;} = null;
        public bool bThisItemRequiresDate {get;set;} = false;
        public DateTime? FirstRecording {get;set;} = null;
        public DateTime? SecondRecording {get;set;} = null;
    }
}