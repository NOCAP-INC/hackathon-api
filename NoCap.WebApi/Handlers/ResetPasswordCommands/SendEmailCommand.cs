using MediatR;
using NoCap.Request;

namespace NoCap.Handlers;

public class SendEmailCommand : IRequest, IRequest<Unit>
{
    public SendEmailCommand(EmailRequest emailRequest)
    {
        EmailRequest = emailRequest;
    }

    public EmailRequest EmailRequest { get; }
}