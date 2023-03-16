using MediatR;

namespace NoCap.Request;

public class VerifyResetPasswordCodeRequest : IRequest<Unit>
{
    public string Email { get; set; }
    public int Code { get; set; }
}