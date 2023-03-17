using MediatR;
using Microsoft.AspNetCore.Mvc;
using NoCap.Handlers;
using NoCap.Request;

namespace NoCap.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ResetPasswordController : ControllerBase
{
    private readonly IMediator _mediator;

    public ResetPasswordController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("send-code")]
    public async Task<IActionResult> SendResetPasswordCode([FromBody] EmailResetPasswordRequest emailRequest)
    {
        var command = new SendResetPasswordCodeCommand { Email = emailRequest.Email };
        var result = await _mediator.Send(command);

        if (result) return Ok();

        return BadRequest();
    }

    [HttpPost("verify-code")]
    public async Task<IActionResult> VerifyResetPasswordCode(
        [FromBody] ResetPasswordCodeRequest resetPasswordCodeRequest)
    {
        var query = new VerifyResetPasswordCodeQuery
            { Email = resetPasswordCodeRequest.Email, Code = resetPasswordCodeRequest.Code };
        var result = await _mediator.Send(query);

        if (result) return Ok();

        return BadRequest();
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest resetPasswordRequest)
    {
        var command = new ResetPasswordCommand
            { Email = resetPasswordRequest.Email, NewPassword = resetPasswordRequest.NewPassword };
        var result = await _mediator.Send(command);
        if (result) return Ok();

        return BadRequest();
    }
}