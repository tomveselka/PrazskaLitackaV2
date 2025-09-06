using MediatR;
using PrazskaLitacka.Domain.Entities;
using PrazskaLitacka.Domain.Interfaces;
using PrazskaLitacka.Webapi.Dto;
using static PrazskaLitacka.Webapi.Requests.StationRequests;

namespace PrazskaLitacka.Webapi.Handlers;

public class GetBonusStationsLinesHandler : IRequestHandler<GetBonusStationsLinesQuery, BonusesDto>
{
    private readonly IBonusLineRepository _lineRepository;
    private readonly IBonusStationRepository _stationRepository;
    private readonly IRaceRepository _raceRepository;

    public GetBonusStationsLinesHandler(IBonusStationRepository stationRepository, IBonusLineRepository lineRepository, IRaceRepository raceRepository)
    {
        _lineRepository = lineRepository;
        _stationRepository = stationRepository;
        _raceRepository = raceRepository;
    }
    public async Task<BonusesDto> Handle(GetBonusStationsLinesQuery request, CancellationToken cancellationToken)
    {
        var race = await _raceRepository.GetById(request.raceId);
        List<BonusStation> stations = new List<BonusStation>();
        List<BonusLine> lines = new List<BonusLine>();
        bool visible = false;
        if (race!.BonusStopDisplayTime >= System.DateTime.Now)
        {
            stations = await _stationRepository.GetAllForRace(request.raceId);
            lines = await _lineRepository.GetAllForRace(request.raceId);
            visible = true;            
        }

        return new BonusesDto
        {
            BonusLines = lines,
            BonusStations = stations,
            Visible = visible,
            VisibleFrom = race!.BonusStopDisplayTime,
            RaceId = request.raceId
        };
    }
}
