using PrazskaLitacka.Domain.Entities;

namespace PrazskaLitacka.Webapi.Dto;

public class BonusesDto
{
    public List<BonusLine> BonusLines { get; set; } = new List<BonusLine>();
    public List<BonusStation> BonusStations { get; set; } = new List<BonusStation>();

    public DateTime VisibleFrom { get; set; }
    public bool Visible {  get; set; }

}
