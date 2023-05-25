using MakeYouPro.Bource.CRM.Dal.Models;
using Microsoft.EntityFrameworkCore;

namespace MakeYouPro.Bource.CRM.Dal
{
    public class CRMContext : DbContext
    {
        public DbSet<AccountEntity> Accounts { get; set; }

        public DbSet<LeadEntity> Leads { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            //  builder.UseSqlServer(Environment.GetEnvironmentVariable("CRMContext"));
            //builder.UseSqlServer(Environment.GetEnvironmentVariable("ConnectLocalBourceCrmDB"));
            builder.UseSqlServer(@"Data Source=DESKTOP-GRG9GQS;Initial Catalog=CRM;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False");

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var foreignKey in modelBuilder.Model.GetEntityTypes().SelectMany(m => m.GetForeignKeys()))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.NoAction;
            }

            modelBuilder.Entity<LeadEntity>()
                .Property(l => l.DateCreate)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<AccountEntity>()
                .Property(l => l.DateCreate)
                .HasDefaultValueSql("getdate()");
        }
    }
}