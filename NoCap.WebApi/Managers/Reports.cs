using System;
using System.Collections.Generic;

namespace NoCap.Managers
{
    public class Reports
    {
        private readonly List<Report> _reports = new List<Report>();

        public void AddReport(Report report)
        {
            _reports.Add(report);
        }

        public List<Report> GetReports()
        {
            return _reports;
        }

        public List<Report> GetReportsForDateRange(DateTime startDate, DateTime endDate)
        {
            List<Report> reportsInRange = new List<Report>();
            foreach (var report in _reports)
            {
                if (report.Date >= startDate && report.Date <= endDate)
                {
                    reportsInRange.Add(report);
                }
            }
            return reportsInRange;
        }

        public void ChangeReportStatus(string reportTitle, Status newStatus)
        {
            foreach (var report in _reports)
            {
                if (report.Title == reportTitle)
                {
                    report.Status = newStatus;
                    break;
                }
            }
        }
    }
}