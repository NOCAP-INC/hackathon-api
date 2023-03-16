using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NoCap.Data;
using NoCap.Managers;

namespace NoCap.Service
{
    public class ReportService
    {
        private readonly IdentityContext _dbContext;
        private readonly UserManager<User> _userManager;

        public ReportService(IdentityContext dbContext, UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task<IEnumerable<Report>> GetAllReportsAsync()
        {
            var reports = _dbContext.Reports.Include(r => r.User).ToList();
            return reports;
        }

        public async Task AddReportAsync(Report report)
        {
            await _dbContext.Reports.AddAsync(report);
            var user = await _userManager.FindByIdAsync(report.UserId);
            var userReport = new UserReport { User = user, Report = report, ReportId = report.Id, UserId = user.Id };
            await _dbContext.UserReports.AddAsync(userReport);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateReportAsync(Report report)
        {
            _dbContext.Reports.Update(report);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteReportAsync(int id)
        {
            var report = await _dbContext.Reports.FindAsync(id);
            if (report != null)
            {
                _dbContext.Reports.Remove(report);
                await _dbContext.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<Report>> GetAllUserReports(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var userId = user.Id;
                var userReports = _dbContext.UserReports
                    .Where(ur => ur.UserId == userId)
                    .Include(ur => ur.Report)
                    .ThenInclude(r => r.User)
                    .ToList();
                return userReports.Select(ur => ur.Report);
            }
            return null;
            
        }
    }
}

