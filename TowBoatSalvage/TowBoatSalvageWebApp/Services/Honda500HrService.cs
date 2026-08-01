using System;
using Microsoft.EntityFrameworkCore;
using TowBoatSalvageWebApp.Data;
using TowBoatSalvageWebApp.Models;

namespace TowBoatSalvageWebApp.Services
{
    public class Honda500HrService
    {
        public readonly SalvageDbContext _db;
        public readonly ILogger<Honda500HrService> _logger;

        public Honda500HrService(SalvageDbContext db, ILogger<Honda500HrService> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task AddAsync(Honda500Hr service)
        {
            _db.Honda500HrServices.Add(service);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateServiceDescriptionAsync(Honda500Hr honda)
        {
            _db.Honda500HrServices.Update(honda);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entry = await _db.Honda500HrServices.FindAsync(id);
            if (entry is null)
                return;

            _db.Honda500HrServices.Remove(entry);
            await _db.SaveChangesAsync();
        }

        public async Task<List<Honda500Hr>> GetHonda500HrServiceFromSelectedYear(int year)
        {
            var query = _db.Honda500HrServices.AsNoTracking();
            query = query.Where(h => h.DateCompleted.Year == year);

            return await query
            .Include(h => h.ServiceDescriptions)
            .OrderByDescending(h => h.DateCompleted)
            .ToListAsync();
        }

        public async Task<Honda500Hr?> GetServiceForPdfByIdAsync(int id)
        {
            return await _db
                .Honda500HrServices.AsNoTracking()
                .Include(h => h.ServiceDescriptions)
                .FirstOrDefaultAsync(h => h.Id == id);
        }

        public async Task<Honda500Hr> CreateNewHonda500HrServiceAsync(
            string user,
            int engineHours1,
            int engineHours2,
            string boatNumber
        )
        {
            var entry = new Honda500Hr()
            {
                CompletedBy = user,
                BoatNumber = boatNumber,
                EngineHours1 = engineHours1,
                EngineHours2 = engineHours2,
                DateCompleted = DateTime.Now,
                ServiceDescriptions = new List<ServiceDescription>
                {
                    new ServiceDescription
                    {
                        Description =
                            "Check battery connections (clean connections if needed and document)",
                    },
                    new ServiceDescription
                    {
                        Description = "Test all batteries and place on charge",
                    },
                    new ServiceDescription { Description = "Change Port engine oil and Filter" },
                    new ServiceDescription
                    {
                        Description =
                            "Change Port engine water seperator fuel trainer (LP) - (if equiped)",
                    },
                    new ServiceDescription
                    {
                        Description = "Change Port engine fuel strainer (HP) and o-ring",
                    },
                    new ServiceDescription
                    {
                        Description = "Change Port engine thermostats and check o-rings (flush before changing)",
                    },
                    new ServiceDescription
                    {
                        Description = "Change Port engine exhaust manifold zincs",
                    },
                    new ServiceDescription
                    {
                        Description = "Change Port engine exhaust manifold gaskets",
                    },
                    new ServiceDescription
                    {
                        Description = "Change Port engine exhaust manifold o-ring",
                    },
                    new ServiceDescription
                    {
                        Description = "Clean Port engine block zinc holes (with hand pick and wire brush on drill and then flush)",
                    },
                    new ServiceDescription
                    {
                        Description = "Flush Port engine VST cooling system",
                    },
                    new ServiceDescription
                    {
                        Description = "Change Port engine block zinks",
                    },
                    new ServiceDescription
                    {
                        Description = "Change Port engine spark plugs (check seals, replace as needed and document)",
                    },
                    new ServiceDescription
                    {
                        Description = "Adjust Port engine valves (have the Chief Mechanic check valve adjustments if available)",
                    },
                    new ServiceDescription
                    {
                        Description = "Clean Port engine IAC valve (check gasket, replace if needed)",
                    },
                    new ServiceDescription
                    {
                        Description = "Change Port Gear Case Lube and o-rings",
                    },
                    new ServiceDescription
                    {
                        Description = "Change Port gear case water pump impeller (check water pump housing plate for corrosion)",
                    },
                    new ServiceDescription
                    {
                        Description = "Inspect Port Timing Belt",
                    },
                    new ServiceDescription
                    {
                        Description = "Change Starboard engine oil and filter",
                    },
                    new ServiceDescription
                    {
                        Description = "Change Starboard engine water separator fuel strainer (LP) - (if equiped)",
                    },
                    new ServiceDescription
                    {
                        Description = "Change Starboard engine fuel strainer (HP) and o-ring",
                    },
                    new ServiceDescription
                    {
                        Description = "Change Starboard engine thermostats and check o-rings (flush before changing)",
                    },
                    new ServiceDescription
                    {
                        Description = "Change Starboard engine exhaust manifold zincs",
                    },
                    new ServiceDescription
                    {
                        Description = "Change Starboard engine exhaust manifold gaskets",
                    },
                    new ServiceDescription
                    {
                        Description = "Change Starboard engine exhaust manifold o-ring",
                    },
                    new ServiceDescription
                    {
                        Description = "Clean Starboard engine block zinc holes (with hand pick and wire brush on drill and then flush)",
                    },
                    new ServiceDescription
                    {
                        Description = "Flush Starboard engine VST cooling system",
                    },
                    new ServiceDescription
                    {
                        Description = "Change Starboard engine block zinks",
                    },
                    new ServiceDescription
                    {
                        Description = "Change Starboard engine spark plugs (check seals, replace as needed and document)",
                    },
                    new ServiceDescription
                    {
                        Description = "Adjust Starboard engine valves (have Chief Mechanic check valve adjustments if available)",
                    },
                    new ServiceDescription
                    {
                        Description = "Clean Starboard engine IAC Valve (check gasket, replace if needed)",
                    },
                    new ServiceDescription
                    {
                        Description = "Change Starboard Gear Case Lube and o-rings",
                    },
                    new ServiceDescription
                    {
                        Description = "Change Starboard gear case water pump impellar",
                    },
                    new ServiceDescription
                    {
                        Description = "Inspect Starboard Timing Belt",
                    },
                    new ServiceDescription
                    {
                        Description = "Grease engine and apply corrosion block (with paint brush)",
                    },
                    new ServiceDescription
                    {
                        Description = "Change Port and Starboard vessel fuel filter",
                    },
                    new ServiceDescription
                    {
                        Description = "Check steering operation and fluid level",
                    },
                    new ServiceDescription
                    {
                        Description = "Paint Port and Starboard trim motors - (2 coats)",
                    },
                    new ServiceDescription
                    {
                        Description = "Clean and check bottom paint - (make needed repairs and document)",
                    },
                    new ServiceDescription
                    {
                        Description = "Complete equipment checklist (document on topside checksheet)",
                    },
                    new ServiceDescription
                    {
                        Description = "Fuel Vessel (document on vessel and truck fuel log, reset computer on boat)",
                    },
                    new ServiceDescription
                    {
                        Description = "Clean vessel after a maintenance",
                    },
                    new ServiceDescription
                    {
                        Description = "Quality check, run both engines check for leaks and any issues (run saltaway through engines)",
                    },
                    new ServiceDescription
                    {
                        Description = "Hook up laptop and run Dr. Honda, full check of system and ECU code clear",
                    },
                },
            };

            _db.Honda500HrServices.Add(entry);
            await _db.SaveChangesAsync();

            return entry;
        }
    }
}