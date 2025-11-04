using PrazskaLitacka.Domain.Entities;

namespace PrazskaLitacka.Domain.Interfaces.ServiceInterfaces;

public interface IRaceEvaluationService
{
    public Task<RaceEntry> EvaluateRace(RaceEntry raceEntry);
}
