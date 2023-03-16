using MediatR;

namespace NoCap.Handlers;

public class VerifyResetPasswordCodeQuery : IRequest<bool>
{
    public string Email { get; set; }
    public int Code { get; set; }
}