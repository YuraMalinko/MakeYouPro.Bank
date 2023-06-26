using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using MakeYouPro.Bourse.Rates.Dal.Models;

namespace MakeYouPro.Bourse.Rates.Dal
{
    public class Context: DbContext
    {
    protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer(Environment.GetEnvironmentVariable("BourseRates"));
            ////builder.UseSqlServer(@"Server=localhost;Database=Rates;Trusted_Connection=True;TrustServerCertificate=True;");
        }
        public DbSet<ARSDto> MainARS { get; set; } = null!;
        public DbSet<BGNDto> MainBGN { get; set; }=null;
        public DbSet<USDDto> MainUSD { get; set; }=null;
        public DbSet<RUBDto> MainRUB { get; set; }=null;
        public DbSet<EURDto> MainEUR { get; set; }=null;
        public DbSet<CNYDto> MainCNY { get; set; }=null;
        public DbSet<RSDDto> MainRSD { get; set; }=null;
        public DbSet<JPYDto> MainJPY { get; set; } = null;

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

        //public ApplicationContext() => Database.EnsureCreated();