using System.ComponentModel.DataAnnotations;

namespace NoCap.Request;

public class EmailResetPasswordRequest
{
    [Required] public string Email { get; set; }
}