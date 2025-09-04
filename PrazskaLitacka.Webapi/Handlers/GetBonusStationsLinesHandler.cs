using MediatR;
using PrazskaLitacka.Domain.Interfaces;
using PrazskaLitacka.Webapi.Dto;
using static PrazskaLitacka.Webapi.Requests.StationRequests;

namespace PrazskaLitacka.Webapi.Handlers;

public class GetBonusStationsLinesHandler : IRequestHandler<GetBonusStationsLinesQuery, BonusesDto>
{
    private readonly IBonusLineRepository _lineRepository;
    private readonly IBonusStationRepository _stationRepository;

    public GetBonusStationsLinesHandler(IBonusStationRepository stationRepository, IBonusLineRepository lineRepository)
    {
        _lineRepository = lineRepository;
        _stationRepository = stationRepository;
    }
    public async Task<BonusesDto> Handle(GetBonusStationsLinesQuery request, CancellationToken cancellationToken)
    {
        var stations = await _stationRepository.GetAll();
        var lines = await _lineRepository.GetAll();

        return new BonusesDto
        {
            BonusLines = lines,
            BonusStations = stations,
            Visible = true,
            VisibleFrom = System.DateTime.Now
        };
    }
}
