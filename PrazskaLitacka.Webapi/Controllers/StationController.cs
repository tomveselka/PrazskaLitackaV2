using MediatR;
using Microsoft.AspNetCore.Mvc;
using PrazskaLitacka.Webapi.Dto;
using PrazskaLitacka.Webapi.Handlers;
using static PrazskaLitacka.Webapi.Requests.StationRequests;

namespace PrazskaLitacka.Webapi.Controllers;

[ApiController]
[Route("[controller]")]
public class StationController : ControllerBase
{
    private readonly IMediator _mediator;

    public StationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("GetBonusStationsAndLines")]
    public async Task<ActionResult<BonusesDto>> GetBonusStationsAndLines([FromQuery] string raceId)
    {
        var result = await _mediator.Send(new GetBonusStationsLinesQuery(int.Parse(raceId)));
        return Ok(result);
    }
}
