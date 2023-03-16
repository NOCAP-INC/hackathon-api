using MediatR;
using NoCap.Request;

namespace NoCap.Handlers;

public class SendEmailCommand : IRequest, IRequest<Unit>
{
    public EmailRequest EmailRequest { get; }

    public SendEmailCommand(EmailRequest emailRequest)
    {
        EmailRequest = emailRequest;
    }
}