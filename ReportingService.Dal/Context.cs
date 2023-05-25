using Microsoft.EntityFrameworkCore;
using ReportingService.Dal.Models.CRM;

namespace ReportingService.Dal
{
    public class Context : DbContext
    {
        public DbSet<AccountEntity> Accounts { get; set; }

        public DbSet<LeadEntity> Leads { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {      
        }
    }
}
