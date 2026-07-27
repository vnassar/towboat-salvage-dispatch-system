namespace TowBoatSalvageWebApp.Models
{
    public class VehicleInspection
    {
        public int Id { get; set; }
        public string VesselNumber { get; set; } = string.Empty;
        public string EmployeeOrCaptain { get; set; } = string.Empty;
        public DateTime? DateOfInspection { get; set; } = DateTime.Now;
        public bool bHasBeenDownloaded { get;set;} = false;

        public InitialInspection InitialInspection { get; set; } = new();
        public CaptainsPersonalItems CaptainsPersonalItems { get; set; } = new();
        public SafetyDevices SafetyDevices { get; set; } = new();
        public MountedLights MountedLights { get; set; } = new();
        public Placards Placards { get; set; } = new();
        public Navigation Navigation { get; set; } = new();
        public VesselEquipment VesselEquipment { get; set; } = new();
        public Salvage Salvage { get; set; } = new();
        public Oil Oil { get; set; } = new();
        public OperationalCheck OperationalCheck { get; set; } = new();
    }

    public class InitialInspection
    {
        public int Id { get; set; }
        public bool FloridaDOCCertificate { get; set; }
        public bool FloridaRegistration { get; set; }
        public bool CurrentRegistration { get; set; }
        public bool AlcoholTestStrip { get; set; }
        public DateTime? AlcoholTestStripDate { get; set; }
        public bool VesselsLogBook { get; set; }
    }

    public class CaptainsPersonalItems
    {
        public int Id { get; set; }
        public bool CaptainsUSCGLicence { get; set; }
        public DateTime? USCGLicenseExpires { get; set; }
        public bool ProofOfDrugConsortium { get; set; }
        public DateTime? DrugConsortiumExpires { get; set; }
        public bool AutoInflateLifeJacket { get; set; }
        public DateTime? AutoInflateLifeJacketExpires { get; set; }
        public bool CaptainsDitchBag { get; set; }
        public bool PLBBatteryRegistration { get; set; }
        public DateTime? PLBExpires { get; set; } 
        public bool ClipboardsWithInvoices { get; set; }
        public bool USCGRulesOfTheRoadBook { get; set; }

    }

    public class SafetyDevices
    {
        public int Id { get; set; }
        public bool CaptainCrewType1Offshore { get; set; }
        public bool AdultPFDs { get; set; }
        public bool ChildsPFDs { get; set; }
        public bool ThrowableLifeRing { get; set; }
        public bool ColdWaterSuite { get; set; }
        public bool Flares { get; set; }
        public DateTime? FlaresExpire { get; set; }
        public bool Epirb { get; set; }
        public DateTime? EpirbExpires { get; set; }
        public bool FireExtinguishers { get; set; }
        public bool FirstAidKit { get; set; }
        public bool ThermalBlanket { get; set; }
        public bool HandheldVHF { get; set; }
    }

    public class MountedLights
    {
        public int Id { get; set; }
        public bool Navigation { get; set; }
        public bool Towing { get; set; }
        public bool PublicSafety { get; set; }
        public bool SpotLight { get; set; }
        public bool DeckFlood { get; set; }
        public bool Flashlight { get; set; }
        public bool Horn { get; set; }
        public bool Siren { get; set; }
        public bool FogHorn { get; set; }
        public bool Hailer { get; set; }
    }

    public class Placards
    {
        public int Id { get; set; }
        public bool DischargeOfOil { get; set; }
        public bool DischargeOfGarbage { get; set; }
        public bool NavigationChart { get; set; }
        public bool BoatUSMembershipApplications { get; set; }
    }

    public class Navigation
    {
        public int Id { get; set; }
        public bool GPS { get; set; }
        public bool BackupGPS { get; set; }
        public bool DepthFinder { get; set; }
        public bool Radar { get; set; }
        public bool VHFPrimary { get; set; }
        public bool VHFSecondary { get; set; }
        public bool DVR { get; set; }
        public bool CheckDVRCardForRecording { get; set; }
        public DateTime? FirstRecording { get; set; }
        public DateTime? LastRecording { get; set; }
        public bool WhiskeyOrELectronicCompass { get; set; }
        public bool Binoculars { get; set; }
    }

    public class VesselEquipment
    {
        public int Id { get; set; }
        public bool TowBit { get; set; }
        public bool Knife { get; set; }
        public bool PrimaryTowLine { get; set; }
        public bool SecondaryTowLine { get; set; }
        public bool FourDockLines { get; set; }
        public bool FendersTwo8 { get; set; }
        public bool SpareBattery { get; set; }
        public bool JumperCables { get; set; }
        public bool TwoBilgePumps { get; set; }
        public bool PrimaryAnchor { get; set; }
        public bool BackupAnchor { get; set; }
        public bool BoatHook { get; set; }
        public bool ToolKit { get; set; }
        public bool TwoGasolineCans { get; set; }
        public bool TwoFuelFilters { get; set; }

    }

    public class Salvage
    {
        public int Id { get; set; }
        public bool TwoRulePumps { get; set; }
        public bool Bucket { get; set; }
        public bool Pump { get; set; }
        public bool PVCPipe { get; set; }
        public bool SuctionHose { get; set; }
        public bool Pads { get; set; }
        public bool WoodenPlugs { get; set; }
        public bool FoamFootballs { get; set; }
    }

    public class Oil
    {
        public int Id { get; set; }
        public bool TwoStrokOil { get; set; }
        public bool FourCycleOil { get; set; }
    }

    public class OperationalCheck
    {
        public int Id { get; set; }
        public bool SalvagePump { get; set; }
        public bool ServicePump { get; set; }
        public bool EmptyFuelCarburator { get; set; }
        public bool FillGasTank { get; set; }
        public bool InspectWiring { get; set; }
        public bool VerifyPumpRuns { get; set; }
        public DateTime? DateOfPumpCheck { get; set; }

    }





}
