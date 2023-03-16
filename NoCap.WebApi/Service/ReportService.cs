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
            return await _dbContext.Reports.Include(r => r.User).ToListAsync();
        }
        public async Task<Report> GetReportByIdAsync(int id)
        {
            return await _dbContext.Reports.Include(r => r.User).FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task AddReportAsync(Report report)
        {
            await _dbContext.Reports.AddAsync(report);
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
                var reports = await _dbContext.Reports
                    .Where(r => r.UserId == user.Id)
                    .ToListAsync();
                return reports;
            }
            return Enumerable.Empty<Report>();
        }
    }
}

