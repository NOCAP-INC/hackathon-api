
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NoCap.Managers;
using NoCap.Request;

namespace NoCap.Controllers

{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly AuthManager _authManager;
        public AuthController(IMediator mediator, AuthManager authManager)
        {
            _mediator = mediator;
            _authManager= authManager;
        }

        [HttpPost("register")]
        public async Task Register(RegisterUserRequest request)
        {
            await _mediator.Send(request);
        }

        [HttpPost("login")]
        public async Task Login(LoginUserRequest request)
        {
            await _mediator.Send(request);
        }

        [HttpGet("googleLogin")]
        public IActionResult GoogleLogin()
        {
            return _authManager.GoogleLogin();
        }

        [HttpPost("googleResponse")]
        public async Task GoogleResponse()
        {
            await _authManager.GoogleResponse();
        }
    }
}
