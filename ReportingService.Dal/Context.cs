using Microsoft.EntityFrameworkCore;
using ReportingService.Dal.Models.CRM;
using ReportingService.Dal.Models.TransactionStore;

namespace ReportingService.Dal
{
    public class Context : DbContext
    {
        public DbSet<AccountEntity> Accounts { get; set; }

        public DbSet<LeadEntity> Leads { get; set; }
        public DbSet<TransactionEntity> Transactions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {      
        }
    }
}
