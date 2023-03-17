using MediatR;
using Microsoft.AspNetCore.Identity;
using NoCap.Managers;
using NoCap.Request.ReportRequests;
using NoCap.Service;

namespace NoCap.Handlers.ReportHandlers;

public class UpdateReportHandler : IRequestHandler<UpdateReportRequest, bool>
{
    private readonly ReportService _reportService;
    private readonly UserManager<User> _userManager;

    public UpdateReportHandler(ReportService reportService, UserManager<User> userManager)
    {
        _reportService = reportService;
        _userManager = userManager;
    }

    public async Task<bool> Handle(UpdateReportRequest request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null) return false;

        var report = await _reportService.GetReportByIdAsync(request.Id);
        if (report == null) return false;

        if (report.UserId != user.Id) return false;

        report.Title = request.Title;
        report.Description = request.Description;
        report.IsResolved = request.IsResolved;

        await _reportService.UpdateReportAsync(report);

        return true;
    }
}