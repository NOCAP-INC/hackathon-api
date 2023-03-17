namespace NoCap.Helpers;

public abstract class CodeHelper
{
    public static int GetRandomCode(int length)
    {
        var rnd = new Random();
        var up = (int)Math.Pow(10, length);
        var code = rnd.Next(up);
        if (length > 9) throw new Exception("code should have length lower then 9");
        return code;
    }
}