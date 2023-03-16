
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NoCap.Request;

namespace NoCap.Controllers

{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
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
        
        
        
    }
}
