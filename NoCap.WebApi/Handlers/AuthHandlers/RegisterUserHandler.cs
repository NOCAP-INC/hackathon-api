using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using NoCap.Configs;
using NoCap.Managers;
using NoCap.Request;
using NoCap.Service;

namespace NoCap.Handlers;

public class RegisterUserHandler : IRequestHandler<RegisterUserRequest, RegisterResult>
{
    private readonly SMTPConfig _config;
    private readonly EmailService _emailService;
    private readonly IUserEmailStore<User> _emailStore;
    private readonly ILogger<RegisterUserHandler> _logger;
    private readonly IMemoryCache _memoryCache;
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly IUserStore<User> _userStore;
    private readonly RoleManager<IdentityRole> _roleManager;

    public RegisterUserHandler(
        UserManager<User> userManager,
        IUserStore<User> userStore,
        SignInManager<User> signInManager,
        EmailService emailService,
        ILogger<RegisterUserHandler> logger,
        Config config,
        RoleManager<IdentityRole> roleManager,
        IMemoryCache memoryCache)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _memoryCache = memoryCache;
        _userStore = userStore;
        _emailStore = (IUserEmailStore<User>)_userStore;
        _signInManager = signInManager;
        _config = config.SMTPConfig;
        _logger = logger;
        _emailService = emailService;
    }

    public async Task<RegisterResult> Handle(RegisterUserRequest request,
        CancellationToken cancellationToken)
    {
        var user = new User();
        SetUserProperties(user, request.FullName, request.Email);
        user.Role = "Admin";
        await _userStore.SetUserNameAsync(user, request.Email, CancellationToken.None);
        await _emailStore.SetEmailAsync(user, request.Email, CancellationToken.None);
        var result = await _userManager.CreateAsync(user, request.Password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "Admin");
            await _signInManager.SignInAsync(user, false);
            return new RegisterResult { Success = true };
        }

        var aggregatedErrorMessages = string.Join("\n", result.Errors
            .Select(e => e.Description));

        if (!string.IsNullOrEmpty(aggregatedErrorMessages))
        {
            return new RegisterResult
            {
                ErrorMessage = aggregatedErrorMessages,
                Success = false
            };
            throw new DException.DException(aggregatedErrorMessages);
        }

        return new RegisterResult();
    }

    private void SetUserProperties(User user, string fullName, string email)
    {
        user.FullName = fullName;
        user.Email = email;
        _userStore.SetUserNameAsync(user, email, CancellationToken.None).Wait();
        _emailStore.SetEmailAsync(user, email, CancellationToken.None).Wait();
    }
}