using MediatR;
using NoCap.Service;

namespace NoCap.Handlers;

public class VerifyResetPasswordCodeQueryHandler : IRequestHandler<VerifyResetPasswordCodeQuery, bool>
{
    private readonly UserService _userService;

    public VerifyResetPasswordCodeQueryHandler(UserService userService)
    {
        _userService = userService;
    }

    public async Task<bool> Handle(VerifyResetPasswordCodeQuery request, CancellationToken cancellationToken)
    {
        return await _userService.VerifyResetPasswordCodeAsync(request.Email, request.Code);
    }
}