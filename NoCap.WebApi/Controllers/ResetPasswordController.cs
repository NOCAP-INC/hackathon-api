using MediatR;
using Microsoft.AspNetCore.Mvc;
using NoCap.Managers;
using NoCap.Request;
using NoCap.Service;

namespace NoCap.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResetPasswordController : ControllerBase
    {
        private readonly UserService _userService;

        public ResetPasswordController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("send-code")]
        public async Task<IActionResult> SendResetPasswordCode([FromBody] EmailResetPasswordRequest emailRequest)
        {
            var result = await _userService.SendResetPasswordCodeAsync(emailRequest.Email);

            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpPost("verify-code")]
        public async Task<IActionResult> VerifyResetPasswordCode([FromBody] ResetPasswordCodeRequest resetPasswordCodeRequest)
        {
            var result = await _userService.VerifyResetPasswordCodeAsync(resetPasswordCodeRequest.Email, resetPasswordCodeRequest.Code);

            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }
        
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest resetPasswordRequest)
        {
            var result = await _userService.ResetPasswordAsync(resetPasswordRequest.Email, resetPasswordRequest.NewPassword);

            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }
    }

}