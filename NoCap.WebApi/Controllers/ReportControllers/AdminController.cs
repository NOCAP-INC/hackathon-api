using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NoCap.Managers;

namespace NoCap.Controllers.ReportControllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : Controller
    {
        private readonly UserManager<User> _userManager;
        
        public AdminController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditRole(string email, string roleName)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                var oldRole = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, oldRole);
                await _userManager.AddToRoleAsync(user, roleName);
                return Ok();
            }
            return BadRequest();
        }
    }
}
