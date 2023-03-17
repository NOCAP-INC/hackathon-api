using MediatR;

namespace NoCap.Request.ReportRequests;

public class DeleteReportRequest : IRequest<bool>
{
    public int Id { get; set; }
    public string Email { get; set; }
}