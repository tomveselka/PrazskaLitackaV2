using MediatR;
using Microsoft.Extensions.Logging;
using PrazskaLitacka.Domain.Entities;
using PrazskaLitacka.Domain.Interfaces;
using PrazskaLitacka.Webapi.Handlers;
using PrazskaLitacka.Webapi.Interfaces;
using static PrazskaLitacka.Webapi.Requests.RaceEntryRequests;


namespace PrazskaLitacka.WebApi.Handlers;
public class EvaluateRaceEntryHandler : IRequestHandler<EvaluateRaceEntryCommand, RaceEntry>
{
    private readonly ILogger<EvaluateRaceEntryHandler> _logger;
    private readonly IRaceEvaluationService _raceEvaluationService;

    public EvaluateRaceEntryHandler(ILogger<EvaluateRaceEntryHandler> logger, IRaceEvaluationService raceEvaluationService)
    {
        _logger = logger;
        _raceEvaluationService = raceEvaluationService;
    }

    public Task<RaceEntry> Handle(EvaluateRaceEntryCommand request, CancellationToken cancellationToken)
    {
        return _raceEvaluationService.EvaluateRace(request.entry);
    }
}
