using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NoCap.Request;

namespace NoCap.Managers

{
    public class AuthManager
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserStore<User> _userStore;
        private readonly IUserEmailStore<User> _emailStore;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<AuthManager> _logger;

        public AuthManager(
            UserManager<User> userManager,
            IUserStore<User> userStore,
            SignInManager<User> signInManager,
            ILogger<AuthManager> logger)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = (IUserEmailStore<User>)_userStore;
            _signInManager = signInManager;
            _logger = logger;
        }
        public async Task Register(RegisterUserRequest request)
        {
            var user = new User();
            SetUserProperties(user, request.FullName, request.Email);

            await _userStore.SetUserNameAsync(user, request.Email, CancellationToken.None);
            await _emailStore.SetEmailAsync(user, request.Email, CancellationToken.None);
            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");
                await _signInManager.SignInAsync(user, isPersistent: false);

            }

            string aggregatedErrorMessages = string.Join("\n", result.Errors
                .Select(e => e.Description));

            if (!string.IsNullOrEmpty(aggregatedErrorMessages))
                throw new DException.DException(aggregatedErrorMessages);

        }
        private void SetUserProperties(User user, string fullName, string email)
        {
            user.FullName = fullName;
            user.Email = email;
            _userStore.SetUserNameAsync(user, email, CancellationToken.None).Wait();
            _emailStore.SetEmailAsync(user, email, CancellationToken.None).Wait();

        }


        public async Task Login(LoginUserRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                _logger.LogInformation("User not found");
                throw new DException.DException("User not found");
            }

            var result = await _signInManager.PasswordSignInAsync(request.Email,
     request.Password, false, false);
            if (result.Succeeded)
                _logger.LogInformation("User logged in.");

            else if (result.IsLockedOut)
            {
                _logger.LogWarning("User account locked out.");
                throw new DException.DException("User account locked out");
            }
            else
                throw new DException.DException("Login Error");
        }

        public async Task GoogleResponse()
        {
            ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                _logger.LogInformation("LoginInfo is empty");
                throw new DException.DException("LogInfo is empty");
            }

            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider,
                info.ProviderKey, false);
            if (result.Succeeded)
                _logger.LogInformation("User logged in.");
            else
            {
                var user = new User
                {
                    Email = info.Principal.FindFirst(ClaimTypes.Email).Value,
                    UserName = info.Principal.FindFirst(ClaimTypes.Email).Value
                };

                IdentityResult identityResult = await _userManager.CreateAsync(user);
                if (identityResult.Succeeded)
                {
                    identityResult = await _userManager.AddLoginAsync(user, info);

                    _logger.LogInformation("User created a new account with password.");

                    if (identityResult.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, false);
                    }
                }
                else
                {
                    string aggregatedErrorMessages = string.Join("\n", identityResult.Errors
                          .Select(e => e.Description));

                    if (!string.IsNullOrEmpty(aggregatedErrorMessages))
                        throw new DException.DException(aggregatedErrorMessages);
                }
            }
        }

        public ActionResult GoogleLogin()
        {
            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", "Auth/googleResponse");
            return new ChallengeResult("Google", properties);
        }
    }
}
