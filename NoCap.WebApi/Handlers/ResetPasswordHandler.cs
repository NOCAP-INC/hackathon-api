using Microsoft.AspNetCore.Identity;
using NoCap.Managers;
using NoCap.Request;

public class ResetPasswordHandler
{
    private readonly UserManager<User> _userManager;

    public ResetPasswordHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<ResetPasswordResult> Handle(ResetPasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return new ResetPasswordResult
            {
                Succeeded = false,
                Errors = new List<string> { "User not found" }
            };
        }

        var result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
        if (result.Succeeded)
        {
            return new ResetPasswordResult { Succeeded = true };
        }
        else
        {
            return new ResetPasswordResult
            {
                Succeeded = false,
                Errors = result.Errors.Select(e => e.Description)
            };
        }
    }
}