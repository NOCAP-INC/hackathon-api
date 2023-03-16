namespace NoCap.Managers
{
    public class UserReport
    {
        public string UserId { get; set; }
        public int ReportId { get; set; }
        public User User { get; set; }
        public Report Report { get; set; }
    }
}
