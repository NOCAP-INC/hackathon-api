namespace NoCap.Managers;

public static class Statuses
{
    public static List<Status> All = new List<Status>
    {
        new Status { Id = 1, Name = "In progress" },
        new Status { Id = 2, Name = "Done" },
        new Status { Id = 3, Name = "Blocked" }
    };
}