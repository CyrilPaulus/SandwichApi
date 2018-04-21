using Microsoft.EntityFrameworkCore;

namespace SandwichApi.Models
{

    public class SandwichContext : DbContext
    {

        public SandwichContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=Sandwichs.db");
        }

        public DbSet<User> Users {get; set;}
        public DbSet<Sandwich> Sandwiches {get; set;}
        public DbSet<Transaction> Transactions {get; set;}

    }

}