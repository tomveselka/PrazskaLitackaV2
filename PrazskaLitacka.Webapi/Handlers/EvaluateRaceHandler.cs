using MediatR;
using Microsoft.Extensions.Logging;
using PrazskaLitacka.Domain.Entities;
using PrazskaLitacka.Domain.Interfaces;
using PrazskaLitacka.Webapi.Handlers;
using PrazskaLitacka.Webapi.Interfaces;
using static PrazskaLitacka.Webapi.Requests.RaceEntryRequests;


namespace PrazskaLitacka.WebApi.Handlers;
public class EvaluateRaceHandler : IRequestHandler<EvaluateRaceEntry, RaceEntry>
{
    private readonly IPointsRepository _pointsRepository;
    private readonly ILogger<EvaluateRaceHandler> _logger;
    private readonly IRaceEvaluationService _raceEvaluationService;

    public EvaluateRaceHandler(IPointsRepository pointsRepository, ILogger<EvaluateRaceHandler> logger, IRaceEvaluationService raceEvaluationService)
    {
        _pointsRepository = pointsRepository;
        _logger = logger;
        _raceEvaluationService = raceEvaluationService;
    }

    public Task<RaceEntry> Handle(EvaluateRaceEntry request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
