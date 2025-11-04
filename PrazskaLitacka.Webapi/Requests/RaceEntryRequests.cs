using MediatR;
using PrazskaLitacka.Domain.Entities;

namespace PrazskaLitacka.Webapi.Requests;

public class RaceEntryRequests
{
    public record EvaluateRaceEntryCommand(RaceEntry entry) : IRequest<RaceEntry>;
}
