using MediatR;
using System.ComponentModel.DataAnnotations;

namespace NoCap.Request.ReportRequests
{
    public class CreateReportRequest : IRequest<bool>
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
