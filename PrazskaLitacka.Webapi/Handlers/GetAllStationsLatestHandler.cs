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
    private readonly IStationRepository _stationRepository;
    private readonly IGetPidDataService _pidDataService;
    private readonly ILogger<GetAllStationsLatestHandler> _logger;
    private readonly TimeProvider _time;

    public GetAllStationsLatestHandler(ITechnicalVariablesRepository variablesRepository, IGetPidDataService pidDataService, ILogger<GetAllStationsLatestHandler> logger, IStationRepository stationRepository, TimeProvider time)
    {
        _variablesRepository = variablesRepository;
        _pidDataService = pidDataService;
        _logger = logger;
        _stationRepository = stationRepository;
        _time = time;
    }

    public async Task<List<Station>> Handle(GetAllStationsLatestHandlerQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("STATION-LIST-REQUEST Received request for all stations");
        var techVariables = await _variablesRepository.GetAll();
        DateTime datetimeOfLastUpdate = techVariables.TimeOfLastStationUpdate;

        if (request.enforceUpdate || (_time.GetLocalNow() - datetimeOfLastUpdate).TotalHours > 24)
        {
            _logger.LogInformation("STATION-LIST-UPDATE-BEGIN New list of stations will be downloaded");
            try
            {
                var stops = await _pidDataService.GetStationXmlAsync();
                var stationsListDownloaded = _pidDataService.GetDataForDbInserts(stops);
                await _pidDataService.UpdateTables(stationsListDownloaded);
                _logger.LogInformation("STATION-LIST-UPDATE-SUCCESS Table of stations successfully updated");
            }
            catch (Exception ex)
            {
                _logger.LogError("STATION-LIST-UPDATE-FAILURE Failed to update table of stations. Exception: {ex}", ex);
            }
        }

        var stationsListFromDb = await _stationRepository.GetAll();
        _logger.LogInformation("STATION-LIST-RETURN Returning list of all stations");
        return stationsListFromDb;
    }
}
