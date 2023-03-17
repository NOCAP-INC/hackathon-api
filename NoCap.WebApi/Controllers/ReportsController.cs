using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NoCap.Managers;
using NoCap.Request.ReportRequests;
using NoCap.Service;

namespace NoCap.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ReportService _reportService;
    private readonly UserManager<User> _userManager;

    public ReportsController(ReportService reportService, IMediator mediator, UserManager<User> userManager)
    {
        _mediator = mediator;
        _reportService = reportService;
        _userManager = userManager;
    }

    [HttpPost("AddReport")]
    public async Task<IActionResult> AddReport([FromBody] AddReportRequest request)
    {
        var result = await _mediator.Send(request);
        if (result) return Ok();
        return BadRequest();
    }

    [HttpGet("GetAllReports")]
    public async Task<ActionResult<List<Report>>> GetAllReports()
    {
        var reports = await _reportService.GetAllReportsAsync();
        return Ok(reports);
    }

    [HttpPost("GetAllUserReports")]
    public async Task<ActionResult<List<UserReport>>> GetAllUserReports([FromBody] GetAllUserReportsRequest request)
    {
        var reports = await _reportService.GetAllUserReports(request.Email);
        return Ok(reports);
    }

    [Authorize(Roles = "User,Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateReport(int id, [FromBody] UpdateReportRequest request)
    {
        var report = await _reportService.GetReportByIdAsync(id);
        if (report == null) return NotFound();

        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null || (user.Id != report.UserId && !await _userManager.IsInRoleAsync(user, "Admin")))
            return Forbid();
        report.Description = request.Description;
        await _reportService.UpdateReportAsync(report);
        return Ok();
    }

    [Authorize(Roles = "User,Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteReport(int id, [FromBody] DeleteReportRequest request)
    {
        var report = await _reportService.GetReportByIdAsync(id);
        if (report == null) return NotFound();

        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null || (user.Id != report.UserId && !await _userManager.IsInRoleAsync(user, "Admin")))
            return Forbid();
        await _reportService.DeleteReportAsync(id);
        return Ok();
    }

    [HttpGet("reports/excel")]
    public async Task<IActionResult> GetReportsExcel()
    {
        var excelBytes = await _reportService.GetReportsExcelAsync();
        return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "reports.xlsx");
    }
}