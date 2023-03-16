using MediatR;
using Microsoft.Build.Framework;

namespace NoCap.Request

{
    public class LoginUserRequest : IRequest<LoginResult>
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}