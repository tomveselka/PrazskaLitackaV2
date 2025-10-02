using System.ComponentModel.DataAnnotations;

namespace PrazskaLitacka.Domain.Entities
{
    public class Row
    {
        [Key] 
        public int Id { get; set; }
        public bool StationFromDuplicate { get; set; }
        public int StationFromPoints { get; set; }
        public required string StationFromName { get; set; }
        public bool StationFromBonus { get; set; }
        public bool StationToDuplicate { get; set; }
        public int StationToPoints { get; set; }
        public required string StationToName { get; set; }
        public bool StationToBonus { get; set; }
        public bool LineDuplicate { get; set; }
        public int LinePoints { get; set; }
        public required string LineName { get; set; }
        public required string LineType { get; set; }
        public bool LineBonus { get; set; }
        public DateTimeOffset TimeAdded { get; set; }
        public bool ManRevisionRequired { get; set; }

        public int RaceEntryId { get; set; }
        public RaceEntry RaceEntry { get; set; } = null!;
    }
}
