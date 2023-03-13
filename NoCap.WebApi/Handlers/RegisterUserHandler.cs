using MediatR;
using Microsoft.AspNetCore.Identity;
using NoCap.Managers;
using NoCap.Request;

namespace NoCap.Handlers

{
    public class RegisterUserHandler : IRequestHandler<RegisterUserRequest, RegisterResult>
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserStore<User> _userStore;
        private readonly IUserEmailStore<User> _emailStore;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<AuthManager> _logger;

        public RegisterUserHandler(
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
        public async Task<RegisterResult> Handle(RegisterUserRequest request,
            CancellationToken cancellationToken)
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
                return new RegisterResult { Success = true };
            }

            string aggregatedErrorMessages = string.Join("\n", result.Errors
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
}