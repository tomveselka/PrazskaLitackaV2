using MediatR;
using Microsoft.AspNetCore.Mvc;
using PrazskaLitacka.Domain.Dto;
using PrazskaLitacka.Domain.Exceptions;
using static PrazskaLitacka.Webapi.Requests.UserRequests;

namespace PrazskaLitacka.Webapi.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("GetUserById")]
    public async Task<IActionResult> GetUserById([FromQuery] string userId)
    {
        try
        {
            var user = await _mediator.Send(new GetUserByIdQuery(int.Parse(userId)));
            return Ok(user);
        }
        catch (UserNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (Exception ex) 
        {
            return StatusCode(500, new { error = "Unexpected error occurred" });
        }
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequestDto userDto)
    {
        var registeredUser = await _mediator.Send(new RegisterUserCommand(userDto)); 
        if(registeredUser.Result.Equals("already_exists", StringComparison.OrdinalIgnoreCase))
        {
            return Conflict(new { error = "UserAlreadyExists", message = "A user with this email already exists." });
        }
        return Ok(registeredUser);
    }
}
