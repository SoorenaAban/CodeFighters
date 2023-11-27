using CodeFighters.WebApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;

namespace CodeFighters.WebApi.Data
{
    public class ApiContext : DbContext
    {
        public ApiContext(DbContextOptions<ApiContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<GameQuestion> GameQuestions { get; set; }
        public DbSet<GameAction> GameActions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasMany(u => u.Games).WithMany(g => g.Players);
            modelBuilder.Entity<User>().HasMany(u => u.GamesStarted).WithOne(g => g.StartedBy);
            modelBuilder.Entity<User>().HasMany(u => u.TurnsIn).WithOne(g => g.Turn);


        }


    }
}
