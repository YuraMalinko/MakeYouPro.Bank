using MakeYouPro.Bank.CRM.Dal.Models;
using Microsoft.EntityFrameworkCore;

namespace MakeYouPro.Bank.CRM.Dal
{
    public class CRMContext : DbContext
    {
        public DbSet<AccountEntity> Accounts { get; set; }

        public DbSet<LeadEntity> Leads { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            //  builder.UseSqlServer(Environment.GetEnvironmentVariable("CRMContext"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var foreignKey in modelBuilder.Model.GetEntityTypes().SelectMany(m => m.GetForeignKeys()))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.NoAction;
            }                       
        }
    }
}
