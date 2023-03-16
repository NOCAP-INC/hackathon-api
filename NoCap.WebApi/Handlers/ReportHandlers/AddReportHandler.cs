using MediatR;
using Microsoft.AspNetCore.Identity;
using NoCap.Managers;
using NoCap.Request.ReportRequests;
using NoCap.Service;

namespace NoCap.Handlers.ReportHandlers
{
    public class AddReportHandler : IRequestHandler<AddReportRequest, bool>
    {
        public readonly ReportService _reportService;
        public readonly UserManager<User> _userManager;
        public AddReportHandler(ReportService reportService, UserManager<User> userManager)
        {
            _userManager= userManager;
            _reportService = reportService;
        }
        public async Task<bool> Handle(AddReportRequest request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            var report = new Report();
            report.Description = request.Description;
            report.Title = request.Title;
            report.UserId = user.Id;
            report.CreatedAt = DateTime.UtcNow;
            report.IsResolved = false;
            if (user != null)
            {
                await _reportService.AddReportAsync(report);
                return true;
            }
            return false;
        }
    }
}
