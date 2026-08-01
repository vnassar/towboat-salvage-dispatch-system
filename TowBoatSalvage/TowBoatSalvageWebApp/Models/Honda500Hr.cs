using System;

namespace TowBoatSalvageWebApp.Models
{
    public class Honda500Hr
    {
        public int Id {get; set;}
        public string BoatNumber {get;set;}= string.Empty;
        public int EngineHours1 {get;set;}
        public int EngineHours2 {get;set;}
        public DateTime DateCompleted {get;set;} = DateTime.UtcNow;
        public List<ServiceDescription> ServiceDescriptions {get;set;} = new();
        public string CompletedBy {get;set;} = string.Empty;
        public bool bIsResolved {get;set;} = false;
        public bool bHasBeenDownloaded {get;set;} = false;
    }

    public class ServiceDescription
    {
        public int Id {get;set;}
        public bool bServiceCompleted {get;set;} = false;
        public string Description {get;set;} = string.Empty;
    }


}