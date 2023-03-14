using MediatR;
using Microsoft.Build.Framework;

namespace NoCap.Request

{
    public class CheckCodeRequest : IRequest<CheckCodeResult>
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public int Code { get; set; }
    }
}