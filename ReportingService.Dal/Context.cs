using Microsoft.EntityFrameworkCore;
using ReportingService.Dal.Models.CRM;
using ReportingService.Dal.Models.TransactionStore;
using ReportingService.DAL.ModelsDAL.Commissions;

namespace ReportingService.Dal
{
    public class Context : DbContext
    {
        public DbSet<AccountEntity> Accounts { get; set; }
        public DbSet<TransactionEntity> Transactions { get; set; }
        public DbSet<LeadEntity> Leads { get; set; }

        public DbSet<CommissionEntity> Commissions { get; set; }
        public DbSet<CommissionPerDayEntity> SumCommissionsPerDay { get; set; }
        public DbSet<CommissionPerMonthEntity> SumCommissionsPerMonth { get; set; }
        public DbSet<CommissionPerYearEntity> SumCommissionsPerYear { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {      
        }
    }
}
