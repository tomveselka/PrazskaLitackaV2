using System.ComponentModel.DataAnnotations;

namespace PrazskaLitacka.Domain.Entities
{
    public class Points
    {
        [Key] 
        public int Id { get; set; }
        [Required] 
        public string Name { get; set; } = null!;
        [Required] 
        public int PointsValue { get; set; }
    }
}
