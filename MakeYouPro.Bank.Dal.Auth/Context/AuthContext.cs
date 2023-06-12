using MakeYouPro.Bank.Dal.Auth.Models;
using Microsoft.EntityFrameworkCore;

namespace MakeYouPro.Bank.Dal.Auth.Context
{
    public class AuthContext : DbContext
    {
        public DbSet<UserDal> Users { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Environment.GetEnvironmentVariable("Bank.Auth.Local"));
        }
    }
}
