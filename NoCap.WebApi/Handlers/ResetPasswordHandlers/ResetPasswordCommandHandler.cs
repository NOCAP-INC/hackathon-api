using MediatR;
using NoCap.Service;

namespace NoCap.Handlers;

public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, bool>
{
    private readonly UserService _userService;

    public ResetPasswordCommandHandler(UserService userService)
    {
        _userService = userService;
    }

    public async Task<bool> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        return await _userService.ResetPasswordAsync(request.Email, request.NewPassword);
    }
}