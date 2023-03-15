using MediatR;

namespace NoCap.Request;

public class ForgotPasswordRequest : IRequest<Unit>
{
    public string Email { get; set; }
}