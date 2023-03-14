namespace NoCap.Request;

public class ResetPasswordResult
{
    public bool Succeeded { get; set; }
    public IEnumerable<string> Errors { get; set; }
}