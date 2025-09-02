using System.ComponentModel.DataAnnotations;

namespace PrazskaLitacka.Domain.Entities
{
    public class Row
    {
        [Key] 
        public int Id { get; set; }
        public bool FromDuplicate { get; set; }
        public int FromPoints { get; set; }
        public string? StationFromName { get; set; }
        public string? StationFromId { get; set; }
        public bool FromBonus { get; set; }
        public bool ToDuplicate { get; set; }
        public int ToPoints { get; set; }
        public string? StationToName { get; set; }
        public string? StationToId { get; set; }
        public bool ToBonus { get; set; }
        public bool LineDuplicate { get; set; }
        public int LinePoints { get; set; }
        public string? LineName { get; set; }
        public string? LineType { get; set; }
        public bool LineBonus { get; set; }
        public DateTime TimeAdded { get; set; }
        public bool ManRevisionRequired { get; set; }

        public int RaceEntryId { get; set; }
        public RaceEntry RaceEntry { get; set; } = null!;
    }
}
