using MediatR;

namespace NoCap.Handlers;

public class SendResetPasswordCodeCommand : IRequest<bool>
{
    public string Email { get; set; }
}