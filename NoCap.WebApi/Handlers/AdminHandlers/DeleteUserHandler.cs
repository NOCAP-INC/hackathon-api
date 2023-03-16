using MediatR;
using Microsoft.AspNetCore.Identity;
using NoCap.Managers;
using NoCap.Request.AdminRequests;

namespace NoCap.Handlers.AdminHandlers
{
    public class DeleteUserHandler : IRequestHandler<DeleteUserRequest, bool>
    {
        private readonly UserManager<User> _userManager;
        public DeleteUserHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> Handle(DeleteUserRequest request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
                return true;
            }
            return false;
        }
    }
}
