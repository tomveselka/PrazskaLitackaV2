using PrazskaLitacka.Domain.Entities;

namespace PrazskaLitacka.Webapi.Dto;

public class BonusesDto
{
    public required List<BonusLine> BonusLines { get; set; } = new List<BonusLine>();
    public required List<BonusStation> BonusStations { get; set; } = new List<BonusStation>();

    public required int RaceId { get; set; }
    public DateTime? VisibleFrom { get; set; }
    public required bool Visible {  get; set; }

}
