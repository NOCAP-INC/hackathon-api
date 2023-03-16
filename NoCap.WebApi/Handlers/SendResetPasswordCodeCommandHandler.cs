using MediatR;
using NoCap.Service;

namespace NoCap.Handlers;

public class SendResetPasswordCodeCommandHandler : IRequestHandler<SendResetPasswordCodeCommand, bool>
{
    private readonly UserService _userService;

    public SendResetPasswordCodeCommandHandler(UserService userService)
    {
        _userService = userService;
    }

    public async Task<bool> Handle(SendResetPasswordCodeCommand request, CancellationToken cancellationToken)
    {
        return await _userService.SendResetPasswordCodeAsync(request.Email);
    }
}