using MediatR;

namespace NoCap.Request;

public class ResetPasswordRequest : IRequest<ResetPasswordResult>
{
    public string Email { get; set; }
    public string Token { get; set; }
    public string NewPassword { get; set; }
}