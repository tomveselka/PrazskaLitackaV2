using MediatR;
using Microsoft.AspNetCore.Mvc;
using PrazskaLitacka.Domain.Entities;
using PrazskaLitacka.Webapi.Dto;
using static PrazskaLitacka.Webapi.Requests.RaceEntryRequests;
using static PrazskaLitacka.Webapi.Requests.StationRequests;

namespace PrazskaLitacka.Webapi.Controllers;

[ApiController]
[Route("[controller]")]
public class RaceController : ControllerBase
{
    private readonly IMediator _mediator;

public RaceController(IMediator mediator)
{
    _mediator = mediator;
}

[HttpPost("EvaluateRaceEntry")]
public async Task<IActionResult> EvaluateRaceEntry([FromBody] RaceEntry entry)
{
    var evaluatedEntry = await _mediator.Send(new EvaluateRaceEntryCommand(entry));
    return Ok(evaluatedEntry);
}
}
