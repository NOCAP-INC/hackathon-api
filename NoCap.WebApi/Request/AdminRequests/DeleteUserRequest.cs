using MediatR;
using System.ComponentModel.DataAnnotations;

namespace NoCap.Request.AdminRequests
{
    public class DeleteUserRequest : IRequest<bool>
    {
        [Required]
        public string Email { get; set; }
    }
}
