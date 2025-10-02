using MediatR;
using PrazskaLitacka.Domain.Entities;

namespace PrazskaLitacka.Webapi.Requests;

public class RaceEntryRequests
{
    public record EvaluateRaceEntry(RaceEntry entry) : IRequest<RaceEntry>;
}
