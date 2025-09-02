using System.ComponentModel.DataAnnotations;

namespace PrazskaLitacka.Domain.Entities
{
    public class BonusStation
    {
        [Key] 
        public int Id { get; set; }
        [Required] 
        public string Zone { get; set; } = null!;
        [Required] 
        public string Name { get; set; } = null!;
        [Required] 
        public string StationId { get; set; } = null!;

        public int RaceId { get; set; }
        public Race Race { get; set; } = null!;
    }
}
