using MediatR;
using NoCap.Configs;
using NoCap.Service;

namespace NoCap.Handlers;

public class EmailHandler : IRequestHandler<SendEmailCommand, Unit>
{
    private readonly SMTPConfig _config;
    private readonly EmailService _emailService;

    public EmailHandler(EmailService emailService, Config config)
    {
        _emailService = emailService;
        _config = config.SMTPConfig;
    }

    public async Task<Unit> Handle(SendEmailCommand request, CancellationToken cancellationToken)
    {
        await _emailService.SendEmailAsync(request.EmailRequest, _config);

        return Unit.Value;
    }
}