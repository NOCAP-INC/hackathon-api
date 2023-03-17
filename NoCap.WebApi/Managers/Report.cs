namespace NoCap.Managers;

public class Report
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsResolved { get; set; }
    public string UserId { get; set; }
    public User User { get; set; }
}