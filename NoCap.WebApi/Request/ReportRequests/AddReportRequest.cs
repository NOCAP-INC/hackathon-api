using System.ComponentModel.DataAnnotations;
using MediatR;

namespace NoCap.Request.ReportRequests;

public class AddReportRequest : IRequest<bool>
{
    [Required] public string Title { get; set; }

    [Required] public string Description { get; set; }

    [Required] public string Email { get; set; }
}