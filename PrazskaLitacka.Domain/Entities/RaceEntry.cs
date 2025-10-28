using System.ComponentModel.DataAnnotations;

namespace PrazskaLitacka.Domain.Entities
{
    public class RaceEntry
    {
        [Key]
        public int Id { get; set; }
        [Required] 
        public int RacerId { get; set; }
        public double StationLAndLinesPointsTotal { get; set; } = 0;
        public double GoodDeeds { get; set; } = 0;
        public double Penalties { get; set; } = 0;
        public double PointsTotal { get; set; } = 0;
        public bool VerificationNeeded { get; set; } = false;
        public bool VerifiedManualy { get; set; } = false;
        public string? VerifierId { get; set; }
        public DateTimeOffset? TimeOfReturn { get; set; }

        public int RaceId { get; set; }
        public Race Race { get; set; } = null!;

        public ICollection<Row> Rows { get; set; } = new List<Row>();
    }
}
