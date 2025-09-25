using MediatR;
using PrazskaLitacka.Domain.Entities;
using PrazskaLitacka.Domain.Interfaces;
using PrazskaLitacka.Webapi.Dto;
using PrazskaLitacka.Webapi.Interfaces;
using static PrazskaLitacka.Webapi.Requests.StationRequests;

namespace PrazskaLitacka.Webapi.Handlers;

public class GetAllStationsLatestHandler : IRequestHandler<GetAllStationsLatestHandlerQuery, List<Station>>
{
    private readonly IStationRepository _stationRepository;
    private readonly IGetPidDataService _pidDataService;
    private readonly ILogger<GetAllStationsLatestHandler> _logger;

    public GetAllStationsLatestHandler(IStationRepository stationRepository, IGetPidDataService pidDataService, ILogger<GetAllStationsLatestHandler> logger)
    {
        _stationRepository = stationRepository;
        _pidDataService = pidDataService;
        _logger = logger;
    }

    public Task<List<Station>> Handle(GetAllStationsLatestHandlerQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
