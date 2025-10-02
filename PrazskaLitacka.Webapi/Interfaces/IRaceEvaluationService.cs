using PrazskaLitacka.Domain.Entities;

namespace PrazskaLitacka.Webapi.Interfaces;

public interface IRaceEvaluationService
{
    public Task<RaceEntry> EvaluateRace(RaceEntry raceEntry);
}
