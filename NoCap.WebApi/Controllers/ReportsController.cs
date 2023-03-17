using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoCap.Managers;
using NoCap.Request;
using NoCap.Request.ReportRequests;
using NoCap.Service;

namespace NoCap.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly ReportService _reportService;
        private readonly IMediator _mediator;
        public ReportsController(ReportService reportService, IMediator mediator)
        {
            _mediator = mediator;
            _reportService = reportService;
        }

        [Authorize(Roles = "User,Admin")]
        [HttpPost("AddReport")]
        public async Task<IActionResult> AddReport([FromBody] AddReportRequest request)
        {
            var result = await _mediator.Send(request);
            if (result)
            {
                return Ok();
            }
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
        //[HttpPost("DeleteReport")]
        //public async Task<IActionResult> DeleteReport([FromBody] DeleteReportRequest request)
        //{

        //}
    }
    
}
