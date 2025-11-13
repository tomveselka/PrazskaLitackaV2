using MediatR;
using PrazskaLitacka.Domain.Entities;
using PrazskaLitacka.Domain.Interfaces;
using PrazskaLitacka.Webapi.Dto;
using static PrazskaLitacka.Webapi.Requests.StationRequests;

namespace PrazskaLitacka.Webapi.Handlers.StationHandlers;

public class GetBonusStationsLinesHandler : IRequestHandler<GetBonusStationsLinesQuery, BonusesDto>
{
    private readonly IBonusLineRepository _lineRepository;
    private readonly IBonusStationRepository _stationRepository;
    private readonly IRaceRepository _raceRepository;
    private readonly ILogger<GetBonusStationsLinesHandler> _logger;
    private readonly TimeProvider _time;

    public GetBonusStationsLinesHandler(IBonusStationRepository stationRepository, IBonusLineRepository lineRepository, IRaceRepository raceRepository, ILogger<GetBonusStationsLinesHandler> logger, TimeProvider time)
    {
        _lineRepository = lineRepository;
        _stationRepository = stationRepository;
        _raceRepository = raceRepository;
        _logger = logger;
        _time = time;
    }
    public async Task<BonusesDto> Handle(GetBonusStationsLinesQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("BONUS-LIST-REQUEST Received request for stations and lines for race {race}", request.raceId);
        var race = await _raceRepository.GetById(request.raceId);
        List<BonusStation> stations = new List<BonusStation>();
        List<BonusLine> lines = new List<BonusLine>();
        bool visible = false;
        if (race!.BonusStopDisplayTime <= _time.GetUtcNow())
        {
            stations = await _stationRepository.GetAllForRace(request.raceId);
            lines = await _lineRepository.GetAllForRace(request.raceId);
            visible = true;
            _logger.LogInformation("BONUS-LIST-RETRIEVED Stations and lines for race {race} retrieved", request.raceId);
        }
        else
        {
            _logger.LogInformation("BONUS-LIST-TOO-EARLY No stations retrieved, time to display not yet passed");
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
