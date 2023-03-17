using MediatR;
using Microsoft.AspNetCore.Mvc;
using NoCap.Request;

namespace NoCap.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserRequest request)
    {
        var result = await _mediator.Send(request);
        if (result.Success) return Ok();
        return BadRequest();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserRequest request)
    {
        var result = await _mediator.Send(request);
        if (result.Success) return Ok();
        return BadRequest();
    }
}