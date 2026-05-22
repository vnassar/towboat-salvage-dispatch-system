using Microsoft.EntityFrameworkCore;
using TowBoatSalvageWebApp.Data;


namespace TowBoatSalvageWebApp.Services
{
    public class FuelLogService
    {
        private readonly SalvageDbContext _db;

        public FuelLogService(SalvageDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(FuelLogEntry entry)
        {
            _db.FuelLogs.Add(entry);
            await _db.SaveChangesAsync();
        }

        public async Task<List<BoatFuelSummary>> GetFuelSummaryAsync(int year)
        {
            return await _db.FuelLogs
                .Where(f => f.LogDate.Year == year)
                .GroupBy(f => f.BoatName)
                .Select(g => new BoatFuelSummary
                {
                    BoatName = g.Key,
                    TotalFuel = g.Sum(x => x.Fuel1 + x.Fuel2 + x.GasCans),
                    TotalFuelGasCans = g.Sum(x => x.GasCans),
                    TotalEntries = g.Count()
                })
                .OrderBy(g => g.BoatName)
                .ToListAsync();
        }

        public async Task<List<string>> GetBoatNamesAsync()
        {
            return await _db.FuelLogs
                .Select(f => f.BoatName)
                .Distinct()
                .OrderBy(n => n)
                .ToListAsync();
        }

        public async Task <List<FuelLogEntry>> GetEntriesAsync(string? boatName)
        {
            var query = _db.FuelLogs.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(boatName))
            {
                query = query.Where(f => f.BoatName == boatName);
            }

            return await query
                .OrderByDescending(f => f.LogDate)
                .ToListAsync();
        }

        public async Task UpdateAsync(FuelLogEntry entry)
        {
            _db.FuelLogs.Update(entry);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entry = await _db.FuelLogs.FindAsync(id);
            if (entry is null)
            {
                return;
            }

            _db.FuelLogs.Remove(entry);
            await _db.SaveChangesAsync();
        }

        public async Task<List<FuelLogEntry>> GetEntriesForBoatYearAsync(string boatName, int year)
        {
            return await _db.FuelLogs.AsNoTracking()
                .Where(f => f.BoatName == boatName && f.LogDate.Year == year)
                .OrderBy(f => f.LogDate)
                .ToListAsync();
        }

        public sealed class BoatFuelSummary
        {
            public string BoatName { get; set; } = string.Empty;
            public decimal TotalFuel { get; set; }
            public decimal TotalFuelGasCans { get; set; }
            public int TotalEntries { get; set; }


        }
    }
}
