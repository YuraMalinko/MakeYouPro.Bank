using MakeYouPro.Bourse.CRM.Dal.Models;
using Microsoft.EntityFrameworkCore;

namespace MakeYouPro.Bourse.CRM.Dal
{
    public class CRMContext : DbContext
    {
        public DbSet<AccountEntity> Accounts { get; set; }

        public DbSet<LeadEntity> Leads { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            //  builder.UseSqlServer(Environment.GetEnvironmentVariable("CRMContext"));
            //builder.UseSqlServer(Environment.GetEnvironmentVariable("ConnectLocalBourseCrmDB"));
            //builder.UseSqlServer(@"Data Source=NACH_SIPO\SQLEXPRESS;Initial Catalog=CRM;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False");
            builder
                //.UseLazyLoadingProxies()
                .UseSqlServer(Environment.GetEnvironmentVariable("ConnectLocalBourceCrmDB"));
            //.UseSqlServer(@"Data Source=NACH_SIPO\SQLEXPRESS;Initial Catalog=ebuchka;Integrated Security=True;TrustServerCertificate=true");

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var foreignKey in modelBuilder.Model.GetEntityTypes().SelectMany(m => m.GetForeignKeys()))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.NoAction;
            }

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var isDeletedProp = entityType.FindProperty("IsDeleted");
                if (isDeletedProp != null)
                {
                    isDeletedProp.SetDefaultValue(false);
                }
            }

            modelBuilder.Entity<LeadEntity>()
                .Property(l => l.DateCreate)
                .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<AccountEntity>()
                .Property(l => l.DateCreate)
                .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}