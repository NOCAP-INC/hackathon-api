using MediatR;
using Microsoft.AspNetCore.Mvc;
using NoCap.Configs;
using NoCap.Handlers;
using NoCap.Request;
using NoCap.Service;

namespace NoCap.Controllers;

public class EmailController : ControllerBase
{
    private readonly EmailService _emailService;
    private readonly SMTPConfig _config;

    public EmailController(EmailService emailService, Config config)
    {
        _emailService = emailService;
        _config = config.SMTPConfig;
    }

    [HttpPost("send")]
    public async Task<IActionResult> SendEmailAsync([FromBody] EmailRequest emailRequest, [FromServices] IMediator mediator)
    {
        await mediator.Send(new SendEmailCommand(emailRequest));
        return Ok();
    }
}