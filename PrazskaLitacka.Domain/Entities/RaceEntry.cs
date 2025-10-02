using System.ComponentModel.DataAnnotations;

namespace PrazskaLitacka.Domain.Entities
{
    public class RaceEntry
    {
        [Key]
        public int Id { get; set; }
        [Required] 
        public int RacerId { get; set; }
        public double StationLAndLinesPointsTotal { get; set; }
        public double GoodDeeds { get; set; }
        public double Penalties { get; set; }
        public double PointsTotal { get; set; }
        public bool VerificationNeeded { get; set; }
        public bool VerifiedManualy { get; set; }
        public string? VerifierId { get; set; }
        public DateTimeOffset? TimeOfReturn { get; set; }

        public int RaceId { get; set; }
        public Race Race { get; set; } = null!;

        public ICollection<Row> Rows { get; set; } = new List<Row>();
    }
}
