using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NoCap.Managers;
using System.Reflection.Emit;

namespace NoCap.Data;

public class IdentityContext : IdentityDbContext<User>
{
    public IdentityContext(DbContextOptions<IdentityContext> options)
        : base(options)
    {

    }
    public DbSet<Report> Reports { get; set; }
    public DbSet<UserReport> UserReports { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var connectionString = config.GetConnectionString("IdentityDbContextConnection")
                               ?? throw new InvalidOperationException(
                                   "Connection string 'IdentityDbContextConnection' not found.");

        optionsBuilder.UseSqlServer(connectionString);
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<UserReport>()
        .HasKey(ur => new { ur.UserId, ur.ReportId });

        builder.Entity<UserReport>()
            .HasOne(ur => ur.User)
            .WithMany()
            .HasForeignKey(ur => ur.UserId);

        builder.Entity<UserReport>()
            .HasOne(ur => ur.Report)
            .WithMany()
            .HasForeignKey(ur => ur.ReportId);
    }
}
