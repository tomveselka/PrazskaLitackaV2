using MediatR;
using PrazskaLitacka.Domain.Entities;
using PrazskaLitacka.Domain.Interfaces;
using PrazskaLitacka.Infrastructure.Persistence;
using static PrazskaLitacka.Webapi.Requests.StationRequests;

namespace PrazskaLitacka.Webapi.Handlers.StationHandlers;

public class GetStationsAutocompleteHandler : IRequestHandler<GetStationsAutocompleteQuery, List<Station>>
{
    private readonly IStationRepository _stationRepository;
    private readonly ILogger<GetStationsAutocompleteHandler> _logger;

    public GetStationsAutocompleteHandler(IStationRepository stationRepository, ILogger<GetStationsAutocompleteHandler> logger)
    {
        _stationRepository = stationRepository;
        _logger = logger;
    }

    public async Task<List<Station>> Handle(GetStationsAutocompleteQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("STATION-AUTOCOMPLETE-REQUEST Received request for stations starting with {name}", request.nameStart);
        var list = await _stationRepository.GetByBeginningOfName(request.nameStart, request.page, request.recordsPerPage);
        _logger.LogInformation("STATION-AUTOCOMPLETE-RETURN Returning list of {count} stations", list.Count);
        return list;
    }
}
