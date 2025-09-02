using System.ComponentModel.DataAnnotations;

namespace PrazskaLitacka.Domain.Entities;

public class BonusLine
{
    [Key] 
    public int Id { get; set; }
    [Required] 
    public string Type { get; set; } = null!;
    [Required] 
    public string Name { get; set; } = null!;

    public int RaceId { get; set; }
    public Race Race { get; set; } = null!;
}
