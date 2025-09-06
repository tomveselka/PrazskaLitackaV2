using MediatR;
using PrazskaLitacka.Webapi.Dto;

namespace PrazskaLitacka.Webapi.Requests;

public class StationRequests
{
    public record GetBonusStationsLinesQuery(int raceId) : IRequest<BonusesDto>;
}
