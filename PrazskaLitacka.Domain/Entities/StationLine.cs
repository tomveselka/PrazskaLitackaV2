using System.ComponentModel.DataAnnotations;

namespace PrazskaLitacka.Domain.Entities
{
    public class StationLine
    {
        [Key] 
        public int Id { get; set; }
        [Required] 
        public string Type { get; set; } = null!;
        [Required] 
        public string Name { get; set; } = null!;

        public int StationId { get; set; }
        public Station Station { get; set; } = null!;
    }
}
