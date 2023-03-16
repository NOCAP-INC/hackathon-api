using MediatR;
using Microsoft.Build.Framework;

namespace NoCap.Request

{
    public class EmailRequest : IRequest<bool>
    {
        [Required]
        public string RecipientEmail { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Body { get; set; }
    }
}