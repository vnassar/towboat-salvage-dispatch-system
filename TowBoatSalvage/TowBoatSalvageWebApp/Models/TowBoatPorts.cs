namespace TowBoatSalvageWebApp.Models
{
    public class TowBoatPorts
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int SortOrder { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
