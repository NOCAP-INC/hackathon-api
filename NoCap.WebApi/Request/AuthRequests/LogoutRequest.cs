using MediatR;
using System.ComponentModel.DataAnnotations;

namespace NoCap.Request.AuthRequests
{
    public class LogoutRequest : IRequest<bool>
    {
        [Required] 
        public string Email { get; set; }
    }
}
