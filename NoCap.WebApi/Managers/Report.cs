namespace NoCap.Managers;

public class Report
{
    public string Title { get; set; }
    public Status Status { get; set; }
    public string Description { get; set; }
    public DateTime Date { get; set; }
    public string Author { get; set; }
    public Role AuthorRole { get; set; }
}
