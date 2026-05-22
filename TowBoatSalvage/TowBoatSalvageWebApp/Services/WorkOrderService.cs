using Microsoft.EntityFrameworkCore;
using TowBoatSalvageWebApp.Data;
using TowBoatSalvageWebApp.Models;
using Microsoft.AspNetCore.SignalR;

namespace TowBoatSalvageWebApp.Services
{
    public class WorkOrderService
    {
        private readonly IDbContextFactory<SalvageDbContext> _dbFactory;
        private readonly IHubContext<SalvageHub> _hubContext;

        public WorkOrderService(IDbContextFactory<SalvageDbContext> dbFactory, IHubContext<SalvageHub> hubContext)
        {
            _dbFactory = dbFactory;
            _hubContext = hubContext;
        }

        public async Task AddAsync(WorkOrder workOrder)
        {
            await using var db = await _dbFactory.CreateDbContextAsync();
            db.WorkOrder.Add(workOrder);
            await db.SaveChangesAsync();

            await _hubContext.Clients.All.SendAsync(
                "WorkOrderCreated",
                workOrder.Id,
                workOrder.VesselName ?? string.Empty);
        }

        public async Task <List<WorkOrder>> GetAllAsync()
        {
            await using var db = await _dbFactory.CreateDbContextAsync();
            return await db.WorkOrder
                .AsNoTracking()
                .OrderBy(f => f.RequestDateDisplay)
                .ToListAsync();
        }

        public async Task <List<string>> GetBoatNamesAsync()
        {
            await using var db = await _dbFactory.CreateDbContextAsync();
            return await db.WorkOrder
                .Select(f => f.VesselName)
                .Distinct()
                .OrderBy(n => n)
                .ToListAsync();
        }

        public async Task <List<WorkOrder>> GetWorkOrdersAsync(string? vesselName)
        {
            await using var db = await _dbFactory.CreateDbContextAsync();
            var query = db.WorkOrder.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(vesselName))
            {
                query = query.Where(f => f.VesselName == vesselName);
            }

            return await query
                .OrderByDescending(f => f.RequestDateDisplay)
                .ToListAsync();
        }

        public async Task UpdateAsync(WorkOrder workOrder)
        {
            await using var db = await _dbFactory.CreateDbContextAsync();
            var existing = await db.WorkOrder.FirstOrDefaultAsync(x => x.Id == workOrder.Id);
            if (existing is null) return;

            existing.VesselName = workOrder.VesselName;
            existing.RequestDateDisplay = workOrder.RequestDateDisplay;
            existing.Engine1Hours = workOrder.Engine1Hours;
            existing.Engine2Hours = workOrder.Engine2Hours;
            existing.ReportedIssues = workOrder.ReportedIssues;
            existing.IssueCorrectionThreads = workOrder.IssueCorrectionThreads;
            existing.IssueCorrections = workOrder.IssueCorrections; //added after james requested each issue gets a correction 
            existing.IssueCorrectionsBy = workOrder.IssueCorrectionsBy;
            existing.IsAddingCorrection = workOrder.IsAddingCorrection;
            existing.CorrectionNotes = workOrder.CorrectionNotes;
            existing.FromCrewMember = workOrder.FromCrewMember;
            existing.IsResolved = workOrder.IsResolved;

            //_db.WorkOrder.Update(workOrder);
            //db.WorkOrder.Update(workOrder);
            await db.SaveChangesAsync();
        }

        public async Task SetHasBeenDownloadedAsync(WorkOrder order)
        {
            await using var db = await _dbFactory.CreateDbContextAsync();
            var existing = await db.WorkOrder.FirstOrDefaultAsync(x => x.Id == order.Id);
            if (existing is null) return;

            existing.bHasBeenDownloaded = true;

            await db.SaveChangesAsync();
        }

        public async Task AddCorrectionAsync(int workOrderId, int issueIndex, string author, string text)
        {
            await using var db = await _dbFactory.CreateDbContextAsync();
            var order = await db.WorkOrder.FirstOrDefaultAsync(x => x.Id == workOrderId);
            if (order is null) return;

            if (issueIndex < 0 || issueIndex >= order.ReportedIssues.Count) return;

            // initialize dictionary if it was null (old records)
            order.IssueCorrectionThreads ??= new();

            if (!order.IssueCorrectionThreads.ContainsKey(issueIndex)) order.IssueCorrectionThreads[issueIndex] = new();

            order.IssueCorrectionThreads[issueIndex].Add(new IssueCorrection
            {
                Author = author.Trim(),
                Text = text.Trim(),
                CreatedAtUtc = DateTime.UtcNow
            });

            // keep legacy fields in sync for backward compatability
            SyncLegacyCorrections(order);

            await db.SaveChangesAsync();
        }

        private static void SyncLegacyCorrections(WorkOrder order)
        {
            order.IssueCorrections = new List<string>();
            order.IssueCorrectionsBy = new List<string>();

            for (var i = 0; i < order.ReportedIssues.Count; i++)
            {
                if (order.IssueCorrectionThreads.TryGetValue(i, out var thread) && thread.Count > 0)
                {
                    // combine all corrections into one string for legacy display
                    var combined = string.Join(" | ", thread.Select(c => $"{c.Author}: {c.Text}"));
                    order.IssueCorrections.Add(combined);
                    order.IssueCorrectionsBy.Add(thread.Last().Author);
                }
                else
                {
                    order.IssueCorrections.Add(string.Empty);
                    order.IssueCorrectionsBy.Add(string.Empty);
                }
            }

            //also keep CorrectionNotes in sync
            var lines = order.ReportedIssues
                .Select((issue, i) => $"{i + 1}. {issue} -> {order.IssueCorrections.ElementAtOrDefault(i)}");
            order.CorrectionNotes = string.Join(Environment.NewLine, lines);
        }

        public async Task DeleteAsync(int id)
        {
            await using var db = await _dbFactory.CreateDbContextAsync();

            var workOrder = await db.WorkOrder.FindAsync(id);
            if(workOrder is null)
            {
                return;
            }

            db.WorkOrder.Remove(workOrder);
            await db.SaveChangesAsync();
        }
    }
}
