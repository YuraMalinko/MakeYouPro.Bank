using MakeYouPro.Bourse.CRM.Auth.Dal.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MakeYouPro.Bourse.CRM.Auth.Dal.Context
{
    public class UserContext : IdentityDbContext
    {
        //private readonly IAuthSetting _authSetting;
        private readonly IConfiguration _config;

        public DbSet<UserEntity> Users { get; set; }

        public DbSet<RefreshTokenEntity> RefreshTokens { get; set; }

        public UserContext(/*IAuthSetting authSetting,*/
            IConfiguration config)
        {
            //_authSetting = authSetting;
            _config = config;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Environment.GetEnvironmentVariable("AuthCrmBourseDB"));
            //optionsBuilder.UseSqlServer(Environment.GetEnvironmentVariable("AuthCrmBourseLocalDB"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var foreignKey in modelBuilder.Model.GetEntityTypes().SelectMany(m => m.GetForeignKeys()))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.NoAction;
            }

            modelBuilder.Entity<RefreshTokenEntity>()
                .Property(rt => rt.Created)
                .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<RefreshTokenEntity>()
                .Property(rt => rt.Expires)
                //.HasDefaultValueSql($"DATEADD(MINUTE,+{_authSetting.ExpiresRefreshTpokenMinut}, GETUTCDATE())");
                .HasDefaultValueSql($"DATEADD(MINUTE,+{_config.GetSection("RefreshTokenSettings:ExpiresRefreshTokenMinut").Value}, GETUTCDATE())");
        }
    }
}