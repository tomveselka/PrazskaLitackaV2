using MediatR;
using PrazskaLitacka.Domain.Entities;
using PrazskaLitacka.Webapi.Dto;

namespace PrazskaLitacka.Webapi.Requests;

public class StationRequests
{
    public record GetBonusStationsLinesQuery(int raceId) : IRequest<BonusesDto>;
    public record GetAllStationsLatestCommand(bool enforceUpdate) :IRequest<List<Station>>;
    public record GetStationsAutocompleteQuery(string nameStart, int page, int recordsPerPage) : IRequest<List<Station>>;
}
