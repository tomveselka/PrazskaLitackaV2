using MediatR;
using PrazskaLitacka.Domain.Entities;
using static PrazskaLitacka.Webapi.Requests.StationRequests;

namespace PrazskaLitacka.Webapi.Handlers.StationHandlers;

public class GetStationsAutocompleteHandler : IRequestHandler<GetStationsAutocompleteQuery, List<Station>>
{

    public Task<List<Station>> Handle(GetStationsAutocompleteQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
