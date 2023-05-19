using MakeYouPro.Bank.Dal.Auth.Models;
using Microsoft.EntityFrameworkCore;

namespace MakeYouPro.Bank.Dal.Auth.Context
{
    public class AuthContext : DbContext
    {
        public DbSet<UserDal> Users { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=DESKTOP-TO5LEQA\SQLEXPRESS;Initial Catalog = Bank.Auth.Local; Integrated Security = True; Persist Security Info = False; Pooling = False; MultipleActiveResultSets = False; Connect Timeout = 60; Encrypt = False; TrustServerCertificate = False");
        }
    }
}
