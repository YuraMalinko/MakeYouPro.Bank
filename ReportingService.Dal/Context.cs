using Microsoft.EntityFrameworkCore;
using ReportingService.Dal.Models.CRM;
using ReportingService.DAL.ModelsDAL.Commissions;

namespace ReportingService.Dal
{
    public class Context : DbContext
    {
        public DbSet<AccountEntity> Accounts { get; set; }

        public DbSet<LeadEntity> Leads { get; set; }

        public DbSet<CommissionEntity> Commissions { get; set; }
        public DbSet<SumCommissionPerDayEntity> SumCommissionsPerDay { get; set; }
        public DbSet<SumCommissionPerMonthEntity> SumCommissionsPerMonth { get; set; }
        public DbSet<SumCommissionPerYearEntity> SumCommissionsPerYear { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {      
        }
    }
}
