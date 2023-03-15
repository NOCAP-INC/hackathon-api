using MediatR;

namespace NoCap.Request;

public class ResetPasswordRequest : IRequest<bool>
{
    public string Email { get; set; }
    public int Code { get; set; }
    public string NewPassword { get; set; }
}