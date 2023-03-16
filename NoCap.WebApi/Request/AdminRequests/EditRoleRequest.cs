using MediatR;
using System.ComponentModel.DataAnnotations;

namespace NoCap.Request.AdminRequests
{
    public class EditRoleRequest : IRequest<bool>
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Role { get; set; }
    }
}
