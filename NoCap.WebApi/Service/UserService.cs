using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using NoCap.Configs;
using NoCap.Data;
using NoCap.Managers;
using NoCap.Request;

namespace NoCap.Service;

public class UserService
{
    private readonly UserManager<User> _userManager;
    private readonly IMemoryCache _memoryCache;
    private readonly EmailService _emailService;
    private readonly SMTPConfig _smtpConfig;

    public UserService(
        UserManager<User> userManager,
        IMemoryCache memoryCache,
        EmailService emailService,
        Config config)
    {
        _userManager = userManager;
        _memoryCache = memoryCache;
        _emailService = emailService;
        _smtpConfig = config.SMTPConfig;
    }

    public async Task<bool> SendResetPasswordCodeAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return false;
        }

        var code = new Random().Next(1000, 10000);
        var cacheOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
        _memoryCache.Set(email, code, cacheOptions);

        var emailRequest = new EmailRequest()
        {
            Body = $"Your reset password code is: {code}",
            Subject = "Reset Password Code",
            RecipientEmail = email
        };

        await _emailService.SendEmailAsync(emailRequest, _smtpConfig);
        return true;
    }


    public async Task<bool> VerifyResetPasswordCodeAsync(string email, int code)
    {
        if (!_memoryCache.TryGetValue(email, out int cachedCode))
        {
            return false;
        }

        if (code != cachedCode)
        {
            return false;
        }

        return true;
    }
    
    public async Task<bool> ResetPasswordAsync(string email, string newPassword)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return false;
        }

        var code = await _userManager.GeneratePasswordResetTokenAsync(user);
        var decodedCode = WebEncoders.Base64UrlDecode(code);
        var token = Encoding.UTF8.GetString(decodedCode);
        var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

        if (result.Succeeded)
        {
            _memoryCache.Remove(email);
            return true;
        }

        return false;
    }
}


