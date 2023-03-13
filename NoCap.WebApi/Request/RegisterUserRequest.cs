using MediatR;
using Microsoft.Build.Framework;


namespace NoCap.Request

{
    public class RegisterUserRequest : IRequest<RegisterResult>
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}