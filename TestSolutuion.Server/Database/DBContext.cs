using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TestSolutuion.Server.Database.Models;

namespace TestSolutuion.Server.Database
{
    public class SQLiteDataBaseContext : IdentityDbContext<AppUser>
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderElement> OrderElements { get; set; }
        public DbSet<Product> Products { get; set; }

        public SQLiteDataBaseContext() => Database.EnsureCreated();

        public SQLiteDataBaseContext(DbContextOptions<SQLiteDataBaseContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=TestSolutuion.db");
        }
    }
}
