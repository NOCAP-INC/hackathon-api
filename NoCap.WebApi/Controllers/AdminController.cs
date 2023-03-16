using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoCap.Managers;
using NoCap.Request.AdminRequests;

namespace NoCap.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : Controller
    {
        private readonly IMediator _mediator;
        private readonly UserManager<User> _userManager;

        public AdminController(IMediator mediator, UserManager<User> userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
        }
        [HttpPost("EditRole")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditRole([FromBody] EditRoleRequest request)
        {
            var result = await _mediator.Send(request);
            if (result)
            {
                return Ok();
            }
            return BadRequest();
        }
        [HttpPost("DeleteUser")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser([FromBody] DeleteUserRequest request)
        {
            var result = await _mediator.Send(request);
            if (result)
            {
                return Ok();
            }
            return BadRequest();
        }
        [HttpGet("GetAllUsers")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            if (users != null)
            {
                return Ok(users);
            }
            return BadRequest();
        }
    }
}
