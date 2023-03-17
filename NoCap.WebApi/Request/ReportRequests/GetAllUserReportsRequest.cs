using MediatR;
using Microsoft.Build.Framework;

namespace NoCap.Request.ReportRequests;

public class GetAllUserReportsRequest : IRequest<bool>
{
    [Required] public string Email { get; set; }
}