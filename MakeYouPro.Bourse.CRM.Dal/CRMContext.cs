
using EntityFrameworkCore.EncryptColumn.Extension;
using EntityFrameworkCore.EncryptColumn.Interfaces;
using EntityFrameworkCore.EncryptColumn.Util;
using MakeYouPro.Bourse.CRM.Dal.Models;
using Microsoft.EntityFrameworkCore;

namespace MakeYouPro.Bourse.CRM.Dal
{
    public class CRMContext : DbContext
    {
        public DbSet<AccountEntity> Accounts { get; set; }

        public DbSet<LeadEntity> Leads { get; set; }

        private readonly IEncryptionProvider _provider;

        //public CRMContext()
        //{
        //    _provider = new GenerateEncryptionProvider("encryptKey7/P+2-");
        //}

        public CRMContext(string encryptKey)
        {
            _provider = new GenerateEncryptionProvider(encryptKey);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            //builder.UseSqlServer(Environment.GetEnvironmentVariable("ConnectLocalBourseCrmDB"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.UseEncryption(_provider);

            foreach (var foreignKey in modelBuilder.Model.GetEntityTypes().SelectMany(m => m.GetForeignKeys()))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.NoAction;
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