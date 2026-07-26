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

        public async Task AddAsync(VehicleInspection inspection)
        {
            _db.VehicleInspection.Add(inspection);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entry = await _db.VehicleInspection.FindAsync(id);
            if (entry is null)
                return;

            _db.VehicleInspection.Remove(entry);
            await _db.SaveChangesAsync();
        }

        public async Task<List<VehicleInspection>> GetVehicleInspectionsFromSelectedYearAsync(
            int year
        )
        {
            var query = _db.VehicleInspection.AsNoTracking();

            query = query.Where(v => v.DateOfInspection!.Value.Year == year);

            return await query.OrderByDescending(v => v.DateOfInspection).ToListAsync();
        }
    }
}
