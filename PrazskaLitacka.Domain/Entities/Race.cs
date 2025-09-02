using System.ComponentModel.DataAnnotations;

namespace PrazskaLitacka.Domain.Entities
{
    public class Race
    {
        [Key] 
        public int Id { get; set; }
        [Required] 
        public DateOnly Date { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        [Required] 
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string? Place { get; set; }
        public string? Coordinates { get; set; }
        public DateTime? BonusStopDisplayTime { get; set; }
        public bool RegistrationOpen { get; set; }
        public bool AcceptsResults { get; set; }

        public ICollection<RaceEntry> RaceEntries { get; set; } = new List<RaceEntry>();
        public ICollection<BonusStation> BonusStations { get; set; } = new List<BonusStation>();
        public ICollection<BonusLine> BonusLines { get; set; } = new List<BonusLine>();
    }
}
