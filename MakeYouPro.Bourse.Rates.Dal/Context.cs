using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using MakeYouPro.Bourse.Rates.Dal.Models;
using Microsoft.Data.SqlClient;
using System.Diagnostics.Metrics;


namespace MakeYouPro.Bourse.Rates.Dal
{
    public class Context: IdentityDbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            //builder.UseSqlServer(Environment.GetEnvironmentVariable("Rates"));
            builder.UseSqlServer(@"Server=localhost;Database=Rates;Trusted_Connection=True;TrustServerCertificate=True;");
        }
        public DbSet<ARSDto> MainARS { get; set; }
        public DbSet<BGNDto> MainBGN { get; set; }
        public DbSet<USDDto> MainUSD { get; set; }
        public DbSet<RURDto> MainRUR { get; set; }
        public DbSet<EURDto> MainEUR { get; set; }
        public DbSet<CNYDto> MainCNY { get; set; }
        public DbSet<RSDDto> MainRSD { get; set; }
        public DbSet<JPYDto> MainJPY { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            foreach (var fkey in builder.Model.GetEntityTypes().SelectMany(k => k.GetForeignKeys()))
            {
                fkey.DeleteBehavior = DeleteBehavior.NoAction;
            }

        }

    }
}
