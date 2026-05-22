using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TowBoatSalvageWebApp.Data
{
    public class FuelLogEntry
    {
        public int ID { get; set; }

        [Required, MaxLength(100)]
        public string BoatName { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string CrewMember { get; set; } = string.Empty;

        [Required]
        public DateTime LogDate { get; set; }

        public decimal Engine1Hours { get; set; }
        public decimal Engine2Hours { get; set; }

        public decimal Fuel1 { get; set; }
        public decimal Fuel2 { get; set; }

        public decimal GasCans { get; set; }

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        [NotMapped]
        public decimal TotalFuel => Fuel1 + Fuel2 + GasCans;
    }
}
