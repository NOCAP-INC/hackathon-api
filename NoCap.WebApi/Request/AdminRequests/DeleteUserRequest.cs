using System.ComponentModel.DataAnnotations;
using MediatR;

namespace NoCap.Request.AdminRequests;

public class DeleteUserRequest : IRequest<bool>
{
    [Required] public string Email { get; set; }
}