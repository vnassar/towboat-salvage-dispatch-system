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
                },
            };

            _db.Honda500HrServices.Add(entry);
            await _db.SaveChangesAsync();

            return entry;
        }
    }
}