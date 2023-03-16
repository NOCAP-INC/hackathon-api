using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NoCap.Data;
using NoCap.Managers;
using NoCap.Request.AdminRequests;

namespace NoCap.Handlers.AdminHandlers
{
    public class DeleteUserHandler : IRequestHandler<DeleteUserRequest, bool>
    {
        private readonly UserManager<User> _userManager;
        private readonly IdentityContext _context;
        public DeleteUserHandler(UserManager<User> userManager, IdentityContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<bool> Handle(DeleteUserRequest request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user != null)
            {
                var userReports = _context.UserReports.Where(ur => ur.UserId == user.Id);
                _context.UserReports.RemoveRange(userReports);
                _context.SaveChanges();
                await _userManager.DeleteAsync(user);
                return true;
            }
            return false;
        }
    }
}
