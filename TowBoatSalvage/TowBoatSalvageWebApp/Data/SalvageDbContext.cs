using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Text.Json;
using TowBoatSalvageWebApp.Models;

namespace TowBoatSalvageWebApp.Data
{
    public class SalvageDbContext : DbContext
    {
        // These DbSets map to tables in SQLite
        public DbSet<SalvageColumn> Columns { get; set; }
        public DbSet<SalvageRow> Rows {  get; set; }
        public DbSet<SalvageCell> Cells {  get; set; }
        public DbSet<SalvageFile> Files {  get; set; }

        public DbSet<TowBoatPorts> Ports { get; set; }
        public DbSet<TowBoatCaptains> Captains { get; set; }
        public DbSet<FuelLogEntry> FuelLogs { get; set; }
        public DbSet<Honda500Hr> Honda500HrServices {get;set;}
        public DbSet<WorkOrder> WorkOrder { get; set; }

        public DbSet<DocumentSignatureRequest> DocumentSignatureRequests { get; set; }

        public DbSet<PaymentRequest> PaymentRequests { get; set; }
        public DbSet<VehicleInspection> VehicleInspection { get; set; }

        public SalvageDbContext(DbContextOptions<SalvageDbContext> options) : base(options) { }

        // Model configuration for table/column relationships
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TowBoatPorts>()
                .HasData(
                    new TowBoatPorts { Id = 1, Name = "Tampa Bay", SortOrder = 0, IsActive = true},
                    new TowBoatPorts { Id = 2, Name = "Gulfport", SortOrder = 1, IsActive = true}
                );

            modelBuilder.Entity<FuelLogEntry>()
                .HasIndex(f => new { f.BoatName, f.LogDate });

            modelBuilder.Entity<FuelLogEntry>()
                .Property(f => f.Fuel1)
                .HasPrecision(10, 2);

            modelBuilder.Entity<FuelLogEntry>()
                .Property(f => f.Fuel2)
                .HasPrecision(10, 2);

            modelBuilder.Entity<FuelLogEntry>()
                .Property(f => f.Engine1Hours)
                .HasPrecision(10, 2);

            modelBuilder.Entity<FuelLogEntry>()
                .Property(f => f.Engine2Hours)
                .HasPrecision(10, 2);

            // Ensure that for each (Row, Column) pair, there is only one cell
            modelBuilder.Entity<SalvageCell>()
                .HasIndex(c => new { c.RowId, c.ColumnId })
                .IsUnique();

            // Configure cascade deltes or restrics as needed
            modelBuilder.Entity<SalvageCell>()
                .HasMany(c => c.Files)
                .WithOne(f => f.Cell)
                .HasForeignKey(f => f.CellId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DocumentSignatureRequest>()
                .HasIndex(x => x.Token)
                .IsUnique();

            modelBuilder.Entity<PaymentRequest>()
                .Property(p => p.Amount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<PaymentRequest>()
                .HasIndex(p => p.StripePaymentLinkId);

            modelBuilder.Ignore<IssueCorrection>();

            modelBuilder.Entity<WorkOrder>()
                .Property(w => w.IssueCorrectionThreads)
                .HasConversion(
                    // How to write to the database: serialize the dictionary to JSON
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                                        // How to read from the database: deserialize JSON back to the dictionary
                    v => string.IsNullOrWhiteSpace(v)
                        ? new Dictionary<int, List<IssueCorrection>>()
                        : JsonSerializer.Deserialize<Dictionary<int, List<IssueCorrection>>>(v, (JsonSerializerOptions?)null)
                            ?? new Dictionary<int, List<IssueCorrection>>()
                )
                .HasColumnType("TEXT")
                .Metadata.SetValueComparer(
                    new ValueComparer<Dictionary<int, List<IssueCorrection>>>(
                        // Are two values equal? Compare their JSON representations
                        (a, b) => JsonSerializer.Serialize(a, (JsonSerializerOptions?)null) == JsonSerializer.Serialize(b, (JsonSerializerOptions?)null),
                        // Hash code for change tracking
                        v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null).GetHashCode(),
                        // How to create a deep copy (snapshot) for change tracking
                        v => JsonSerializer.Deserialize<Dictionary<int, List<IssueCorrection>>>(
                            JsonSerializer.Serialize(v, (JsonSerializerOptions?)null), (JsonSerializerOptions?)null)
                            ?? new Dictionary<int, List<IssueCorrection>>()
                )
            );

            modelBuilder.Entity<Honda500Hr>()
                .HasMany(h => h.ServiceDescriptions)
                .WithOne()  // ServiceDescription doesn't need to reference back to Honda500Hr
                .HasForeignKey("Honda500HrId")  // Foreign key in ServiceDescription table
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
