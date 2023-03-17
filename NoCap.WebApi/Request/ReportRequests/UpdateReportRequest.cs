using MediatR;

namespace NoCap.Request.ReportRequests;

public class UpdateReportRequest : IRequest<bool>
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsResolved { get; set; }
    public string Email { get; set; }
}