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
    private readonly IMediator _mediator;

    public EmailController(EmailService emailService, Config config, IMediator mediator)
    {
        _emailService = emailService;
        _config = config.SMTPConfig;
        _mediator = mediator;
    }

    [HttpPost("send")]
    public async Task<IActionResult> SendEmailAsync([FromBody] EmailRequest emailRequest, [FromServices] IMediator mediator)
    {
        await mediator.Send(new SendEmailCommand(emailRequest));
        return Ok();
    }
}