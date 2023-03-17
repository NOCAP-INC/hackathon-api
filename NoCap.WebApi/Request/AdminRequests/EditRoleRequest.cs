using System.ComponentModel.DataAnnotations;
using MediatR;

namespace NoCap.Request.AdminRequests;

public class EditRoleRequest : IRequest<bool>
{
    [Required] public string Email { get; set; }

    [Required] public string Role { get; set; }
}