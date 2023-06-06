//using EntityFrameworkCore.EncryptColumn.Example.Entity;
using Bogus;
using EntityFrameworkCore.EncryptColumn.Extension;
using EntityFrameworkCore.EncryptColumn.Interfaces;
using EntityFrameworkCore.EncryptColumn.Util;
using MakeYouPro.Bource.CRM.Dal.Models;
using MakeYouPro.Bourse.CRM.Dal;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace MakeYouPro.Bource.CRM.Dal
{
    public class CRMContext : DbContext
    {
        public DbSet<AccountEntity> Accounts { get; set; }

        public DbSet<LeadEntity> Leads { get; set; }

        private readonly IEncryptionProvider _provider;

        public CRMContext(string encryptKey)
        {
            this._provider = new GenerateEncryptionProvider(encryptKey);
            foreach (var lead in leads)
            {
                this.Leads.Add(lead);
            };
            foreach (var account in accounts)
            {
                this.Accounts.Add(account);
            };
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            //  builder.UseSqlServer(Environment.GetEnvironmentVariable("CRMContext"));
            //builder.UseSqlServer(Environment.GetEnvironmentVariable("ConnectLocalBourceCrmDB"));
            builder.UseSqlServer(@"Data Source=DESKTOP-GRG9GQS;Initial Catalog=CRM7Generate;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False");

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.UseEncryption(_provider);

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

            //modelBuilder.Entity<LeadEntity>()
            //    .HasMany(e => e.Accounts)
            //    .WithOne(e => e.Lead)
            //    .HasForeignKey(e => e.LeadId);

            modelBuilder.Entity<AccountEntity>()
                .Property(l => l.DateCreate)
                .HasDefaultValueSql("GETUTCDATE()");

        }


        static Faker<LeadEntity> generatorRandomLead = FakerGenerate.GetGeneratorRandomLead();
        //Faker<AccountEntity> generatorAccount = FakerGenerate.GetGeneratorRandomAccount();
        List<LeadEntity> leads = generatorRandomLead.Generate(100);
        List<AccountEntity> accounts = FakerGenerate.GetGeneratorRandomAccount().Generate(100);

    }
}