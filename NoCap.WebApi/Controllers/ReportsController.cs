using MediatR;
using Microsoft.AspNetCore.Mvc;
using NoCap.Managers;
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
        []

        [HttpGet("GetAllReports")]
        public async Task<ActionResult<List<Report>>> GetAllReports() 
        {
            var reports = await _reportService.GetAllReportsAsync();

            return Ok(reports);
        }
    }
    
}
