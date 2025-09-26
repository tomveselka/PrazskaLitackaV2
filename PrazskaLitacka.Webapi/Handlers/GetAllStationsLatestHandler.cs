using MediatR;
using PrazskaLitacka.Domain.Entities;
using PrazskaLitacka.Domain.Interfaces;
using PrazskaLitacka.Webapi.Dto;
using PrazskaLitacka.Webapi.Interfaces;
using static PrazskaLitacka.Webapi.Requests.StationRequests;

namespace PrazskaLitacka.Webapi.Handlers;

public class GetAllStationsLatestHandler : IRequestHandler<GetAllStationsLatestHandlerQuery, List<Station>>
{
    private readonly ITechnicalVariablesRepository _variablesRepository;
    private readonly IGetPidDataService _pidDataService;
    private readonly ILogger<GetAllStationsLatestHandler> _logger;

    public GetAllStationsLatestHandler(ITechnicalVariablesRepository variablesRepository, IGetPidDataService pidDataService, ILogger<GetAllStationsLatestHandler> logger)
    {
        _variablesRepository = variablesRepository;
        _pidDataService = pidDataService;
        _logger = logger;
    }

    public async Task<List<Station>> Handle(GetAllStationsLatestHandlerQuery request, CancellationToken cancellationToken)
    {
        var stops = await _pidDataService.GetStationXmlAsync();
        var stationsList = _pidDataService.GetDataForDbInserts(stops);
        var techVariables = await _variablesRepository.GetAll();
        DateTime datetimeOfLastUpdate = techVariables.TimeOfLastStationUpdate;

        if (request.enforceUpdate || (DateTime.Now - datetimeOfLastUpdate).TotalHours > 24)
        {
            await _pidDataService.UpdateTables(stationsList);
        }

        return stationsList;
    }
}
