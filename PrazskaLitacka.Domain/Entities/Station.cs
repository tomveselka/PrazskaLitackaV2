using System.ComponentModel.DataAnnotations;

namespace PrazskaLitacka.Domain.Entities
{
    public class Station
    {
        [Key] 
        public int Id { get; set; }
        [Required] 
        public string Name { get; set; } = null!;
        [Required] 
        public float AvgLat { get; set; }
        [Required] 
        public float AvgLon { get; set; }
        [Required] 
        public string Zones { get; set; } = null!;

        public ICollection<StationLine> Lines { get; set; } = new List<StationLine>();
    }
}
