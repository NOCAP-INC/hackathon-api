using MediatR;
using Microsoft.AspNetCore.Identity;
using NoCap.Managers;
using NoCap.Request.AdminRequests;

namespace NoCap.Handlers.AdminHandlers;

public class EditRoleCommandHandler : IRequestHandler<EditRoleRequest, bool>
{
    private readonly UserManager<User> _userManager;

    public EditRoleCommandHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<bool> Handle(EditRoleRequest request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user != null)
        {
            user.Role = request.Role;
            var oldRole = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, oldRole);
            await _userManager.AddToRoleAsync(user, request.Role);
            return true;
        }

        return false;
    }
}