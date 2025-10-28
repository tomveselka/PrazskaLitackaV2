using System.ComponentModel.DataAnnotations;

namespace PrazskaLitacka.Domain.Entities
{
    public class Row
    {
        [Key] 
        public int Id { get; set; }
        public bool StationFromDuplicate { get; set; } = false;
        public int StationFromPoints { get; set; } = 0;
        public required string StationFromName { get; set; }
        public bool StationFromBonus { get; set; } = false;
        public bool StationToDuplicate { get; set; } = false;
        public int StationToPoints { get; set; } = 0;
        public required string StationToName { get; set; }
        public bool StationToBonus { get; set; } = false;
        public bool LineDuplicate { get; set; } = false;
        public int LinePoints { get; set; } = 0;
        public required string LineName { get; set; }
        public required string LineType { get; set; }
        public bool LineBonus { get; set; } = false;
        public DateTimeOffset TimeAdded { get; set; }
        public bool ManRevisionRequired { get; set; } = false;

        public int RaceEntryId { get; set; }
        public RaceEntry RaceEntry { get; set; } = null!;
    }
}
