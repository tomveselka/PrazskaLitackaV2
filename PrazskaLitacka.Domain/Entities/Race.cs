using System.ComponentModel.DataAnnotations;

namespace PrazskaLitacka.Domain.Entities
{
    public class Race
    {
        [Key] 
        public int Id { get; set; }
        [Required] 
        public DateOnly Date { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset EndTime { get; set; }
        [Required] 
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string? Place { get; set; }
        public string? Coordinates { get; set; }
        public DateTimeOffset BonusStopDisplayTime { get; set; }
        public bool RegistrationOpen { get; set; } = false;
        public bool AcceptsResults { get; set; } = false;

        public ICollection<RaceEntry>? RaceEntries { get; set; } = new List<RaceEntry>();
        public ICollection<BonusStation>? BonusStations { get; set; } = new List<BonusStation>();
        public ICollection<BonusLine>? BonusLines { get; set; } = new List<BonusLine>();
    }
}
