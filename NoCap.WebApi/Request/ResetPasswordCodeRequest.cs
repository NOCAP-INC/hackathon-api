namespace NoCap.Request;

public class ResetPasswordCodeRequest
{
    public string Email { get; set; }
    public int Code { get; set; }
}