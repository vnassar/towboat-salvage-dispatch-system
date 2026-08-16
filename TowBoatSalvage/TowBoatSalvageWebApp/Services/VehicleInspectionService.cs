using System;
using Microsoft.EntityFrameworkCore;
using TowBoatSalvageWebApp.Data;
using TowBoatSalvageWebApp.Models;

namespace TowBoatSalvageWebApp.Services
{
    public class VehicleInspectionService
    {
        private readonly SalvageDbContext _db;
        private readonly ILogger<VehicleInspectionService> _logger;

        public VehicleInspectionService(
            SalvageDbContext db,
            ILogger<VehicleInspectionService> logger
        )
        {
            _db = db;
            _logger = logger;
        }

        public async Task AddAsync(VesselInspection inspection)
        {
            _db.VesselInspection.Add(inspection);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entry = await _db.VesselInspection.FindAsync(id);
            if (entry is null)
                return;

            _db.VesselInspection.Remove(entry);
            await _db.SaveChangesAsync();
        }

        public async Task<List<VesselInspection>> GetVehicleInspectionsFromSelectedYearAsync(
            int year
        )
        {
            var query = _db.VesselInspection.AsNoTracking();

            query = query.Where(v => v.DateOfInspection!.Value.Year == year);

            return await query.OrderByDescending(v => v.DateOfInspection).ToListAsync();
        }

        public async Task<VesselInspection?> GetInspectionForPdfByIdAsync(int id)
        {
            return await _db.VesselInspection
                .AsNoTracking()
                .Include(v => v.ServiceDescriptions)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task UpdateAsync(VesselInspection inspection)
        {
            _db.VesselInspection.Update(inspection); 
            await _db.SaveChangesAsync();
        }

        public async Task<VesselInspection> CreateNewVesselInspectionAsync(string user, string boatNumber, DateTime? date, int? engine1Hours, int? engine2Hours)
        {
            var inspection = new VesselInspection()
            {
                CompletedBy = user,
                BoatNumber = boatNumber,
                DateOfInspection = date,
                Engine1Hours = engine1Hours,
                Engine2Hours = engine2Hours,
                Notes = string.Empty,
                ServiceDescriptions = new List<ServiceDescriptionVesselInspection>
                {
                    new ServiceDescriptionVesselInspection
                    {
                        Description = "Federal DOC Certificate or"
                    },
                    new ServiceDescriptionVesselInspection
                    {
                        Description = "Florida registration with 'FL' letters displayed"
                    },
                    new ServiceDescriptionVesselInspection
                    {
                        Description = "Current registration/documentation in PVC tube"
                    },
                    new ServiceDescriptionVesselInspection
                    {
                        Description = "Alcohol test strip in PVC tube", bThisItemRequiresDate = true
                    },
                    new ServiceDescriptionVesselInspection
                    {
                        Description = "Vessel's log book (fuel/maintenance) onboard vessel at all times"
                    },
                    new ServiceDescriptionVesselInspection
                    {
                        Description = "Captain's USCG License", bThisItemRequiresDate = true
                    },
                    new ServiceDescriptionVesselInspection
                    {
                        Description = "Proof of Drug Consortium Enrollment", bThisItemRequiresDate = true
                    },
                    new ServiceDescriptionVesselInspection
                    {
                        Description = "Auto-inflate lifejacket", bThisItemRequiresDate = true
                    },
                    new ServiceDescriptionVesselInspection
                    {
                        Description = "Captain's Ditch Bag"
                    },
                    new ServiceDescriptionVesselInspection
                    {
                        Description = "PLB (Battery & Registration)", bThisItemRequiresDate = true
                    },
                    new ServiceDescriptionVesselInspection
                    {
                        Description = "Clipboard with invoices, salvage contracts, ungrounding R.O.L"
                    },
                    new ServiceDescriptionVesselInspection
                    {
                        Description = "USCG Rules of the Road Book"
                    },
                    new ServiceDescriptionVesselInspection
                    {
                        Description = "Captain/Crew - Type 1 offshore (One oneboard)"
                    },
                    new ServiceDescriptionVesselInspection
                    {
                        Description = "Adult - Type III PFD'S (or type 1)(four onboard)"
                    },
                    new ServiceDescriptionVesselInspection
                    {
                        Description = "Childs - Type III PFD'S (two onboard)"
                    },
                    new ServiceDescriptionVesselInspection
                    {
                        Description = "Throwable (life ring/float cushion) - one minimum on board"
                    },
                    new ServiceDescriptionVesselInspection
                    {
                        Description = "Cold water immersion suit"
                    },
                    new ServiceDescriptionVesselInspection
                    {
                        Description = "Pyro/Flares: 3 'Day' & 'Night'", bThisItemRequiresDate = true
                    },
                    new ServiceDescriptionVesselInspection
                    {
                        Description = "406 EPIRB (Battery & Registration)", bThisItemRequiresDate = true
                    },
                    new ServiceDescriptionVesselInspection
                    {
                        Description = "Fire Extinguishers - two small, one 5LB mounted (inspect)"
                    },
                    new ServiceDescriptionVesselInspection
                    {
                        Description = "First Aid Kit (5 Person)"
                    },
                    new ServiceDescriptionVesselInspection
                    {
                        Description = "Thermal Blanket (foil type)"
                    },
                    new ServiceDescriptionVesselInspection
                    {
                        Description = "Handheld VHF"
                    },
                    new ServiceDescriptionVesselInspection
                    {
                        Description = "Navigation"
                    },
                    new ServiceDescriptionVesselInspection
                    {
                        Description = "Towing (yellow over white)"
                    },
                    new ServiceDescriptionVesselInspection
                    {
                        Description = "Public Safety (red, amber, white strobes)"
                    },
                    new ServiceDescriptionVesselInspection
                    {
                        Description = "Spot Light"
                    },
                    new ServiceDescriptionVesselInspection
                    {
                        Description = "Deck flood/cockpit lights"
                    },
                    new ServiceDescriptionVesselInspection
                    {
                        Description = "Flashlight"
                    },
                    new ServiceDescriptionVesselInspection
                    {
                        Description = "Horn/sound producing device"
                    },
                    new ServiceDescriptionVesselInspection
                    {
                        Description = "Siren"
                    },
                    new ServiceDescriptionVesselInspection
                    {
                        Description = "Fog Horn"
                    },
                    new ServiceDescriptionVesselInspection
                    {
                        Description = "Hailer"
                    },
                    new ServiceDescriptionVesselInspection
                    {
                        Description = "Discharge of Oil Placard"
                    },
                    new ServiceDescriptionVesselInspection
                    {
                        Description = "Discharge of Garbage Placard"
                    },
                    new ServiceDescriptionVesselInspection
                    {
                        Description = "Navigation Chart, electronic or paper"
                    },
                    new ServiceDescriptionVesselInspection
                    {
                        Description = "BoatU.S. Membership Applications"
                    },
                    new ServiceDescriptionVesselInspection
                    {
                        Description = "GPS with Electronic navigtation charts"
                    },
                    new ServiceDescriptionVesselInspection
                    {
                        Description = "Backup GPS"
                    },
                    new ServiceDescriptionVesselInspection
                    {
                        Description = "Depth Finder"
                    },
                    new ServiceDescriptionVesselInspection
                    {
                        Description = "Radar"
                    },
                    new ServiceDescriptionVesselInspection
                    {
                        Description = "VHF Primary w/MMSI/GPS"
                    },
                    new ServiceDescriptionVesselInspection
                    {
                        Description = "VHF Secondary"
                    },
                    new ServiceDescriptionVesselInspection
                    {
                        Description = "DVR system operational"
                    },
                    new ServiceDescriptionVesselInspection
                    {
                        Description = "Check DVR Card for recording on pc", bThisItemRequiresDate = true
                    },
                    new ServiceDescriptionVesselInspection
                    {
                        Description = "Whiskey or Electronic Compass"
                    },
                    new ServiceDescriptionVesselInspection
                    {
                        Description = "Binoculars"
                    },
                    new ServiceDescriptionVesselInspection
                    {
                        Description = "Tow Bit - Inspect no cracks, bolts tight"
                    },
                    new ServiceDescriptionVesselInspection
                    {
                        Description = "Knife mounted on tow-post for quick access"
                    },
                    new ServiceDescriptionVesselInspection
                    {
                        Description = "Primary Tow Line - min 400' of 3/4' three strand Poly Pro (inspect)"
                    },
                    new ServiceDescriptionVesselInspection
                    {
                        Description = "Secondary Tow Line - min of 200' of 3/4' three strand Poly Pro (inspect)"
                    },
                    new ServiceDescriptionVesselInspection
                    {
                        Description = "Fenders Two 8' (Min cylinders min two round all purpose 20' diameter"
                    },
                    new ServiceDescriptionVesselInspection
                    {
                        Description = "Spare Battery"
                    },
                    new ServiceDescriptionVesselInspection
                    {
                        Description = "Jumper Cables"
                    },
                    new ServiceDescriptionVesselInspection
                    {
                        Description = "Two operational 2000 GPH bilge pumps installed (works on manual & auto)"
                    },
                    new ServiceDescriptionVesselInspection
                    {
                        Description = "Primary anchor with min 150' rode+chain+anchor"
                    },
                    new ServiceDescriptionVesselInspection
                    {
                        Description = "Backup anchor with min 100' rode+anchor"
                    },
                    new ServiceDescriptionVesselInspection
                    {
                        Description = "Boat Hook"
                    },
                    new ServiceDescriptionVesselInspection
                    {
                        Description = "Tool kit - to include fuel filter wrench"
                    },
                    new ServiceDescriptionVesselInspection
                    {
                        Description = "Two five/six gallon gasolione cans"
                    },
                    new ServiceDescriptionVesselInspection
                    {
                        Description = "Two water-separating fuel filters (un-opened spares)"
                    },
                    new ServiceDescriptionVesselInspection
                    {
                        Description = "Two 3700 GPH Rule DC pumps"
                    },
                    new ServiceDescriptionVesselInspection
                    {
                        Description = "One 3-to-5-gallon bucket w/ sponge"
                    },
                    new ServiceDescriptionVesselInspection
                    {
                        Description = "One 2' engine driven pump"
                    },
                    new ServiceDescriptionVesselInspection
                    {
                        Description = "Discharge PVC pipe (for 2' gas pump)"
                    },
                    new ServiceDescriptionVesselInspection
                    {
                        Description = "Suction hose (for 2' gas pump)"
                    },
                    new ServiceDescriptionVesselInspection
                    {
                        Description = "Oil absorbent pads (15 to 20)"
                    },
                    new ServiceDescriptionVesselInspection
                    {
                        Description = "Damage Control - woode plugs (assorted sizes)"
                    },
                    new ServiceDescriptionVesselInspection
                    {
                        Description = "Damage Control - foam footballs/foam chunks"
                    },
                    new ServiceDescriptionVesselInspection
                    {
                        Description = "One gallon two-stroke oil (for sale)"
                    },
                    new ServiceDescriptionVesselInspection
                    {
                        Description = "One quart four-cycle oil (for sale)"
                    },
                    new ServiceDescriptionVesselInspection
                    {
                        Description = "Run salvage pump dockside with suction and discharge hoses attached, establish a pumping condition"
                    },
                    new ServiceDescriptionVesselInspection
                    {
                        Description = "Service pump by rinsing out and draining pump housing"
                    },
                    new ServiceDescriptionVesselInspection
                    {
                        Description = "Empty the fuel/carburetor bowl by running until empty"
                    },
                    new ServiceDescriptionVesselInspection
                    {
                        Description = "Fill gas tank with non-ethanol fuel"
                    },
                    new ServiceDescriptionVesselInspection
                    {
                        Description = "Inspect wiring of 3700 GPH Rule DC Pumps"
                    },
                    new ServiceDescriptionVesselInspection
                    {
                        Description = "Verify 3700 Rule pumps run, inspect wiring for excess heat"
                    },
                    new ServiceDescriptionVesselInspection
                    {
                        Description = "Date of completed pump check",
                        bThisItemRequiresDate = true
                    }
                }
            };

            _db.VesselInspection.Add(inspection);
            await _db.SaveChangesAsync();

            return inspection;
        }
    }
}
