using MediatR;
using PrazskaLitacka.Domain.Entities;
using PrazskaLitacka.Webapi.Dto;

namespace PrazskaLitacka.Webapi.Requests;

public class StationRequests
{
    public record GetBonusStationsLinesQuery(int raceId) : IRequest<BonusesDto>;
    public record GetAllStationsLatestHandlerQuery(bool enforceUpdate) :IRequest<List<Station>>;
}
