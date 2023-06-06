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
            builder.UseSqlServer(Environment.GetEnvironmentVariable("ConnectLocalBourseCrmDB"));
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