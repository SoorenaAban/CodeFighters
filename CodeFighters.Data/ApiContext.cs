using CodeFighters.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;

namespace CodeFighters.Data
{
    public class ApiContext : DbContext
    {
        public ApiContext(DbContextOptions<ApiContext> options) : base(options)
        {
        }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<GameModel> Games { get; set; }
        public DbSet<ReportModel> Reports { get; set; }
        public DbSet<UserMessageModel> Messages { get; set; }
        public DbSet<GameCodeModel> GameCodes { get; set; }
        public DbSet<GameErrorModel> GameErrors { get; set; }
        public DbSet<LogModel> Logs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>().HasMany(u => u.Games).WithMany(g => g.Players);
            modelBuilder.Entity<UserModel>().HasMany(u => u.Reports).WithOne(r => r.ReportedUser);
            modelBuilder.Entity<UserModel>().HasMany(u => u.ReportsMade).WithOne(r => r.ReportingUser);
            modelBuilder.Entity<UserModel>().HasMany(u => u.MessagesReceived).WithOne(c => c.Receiver);
            modelBuilder.Entity<UserModel>().HasMany(u => u.MessagesSent).WithOne(c => c.Sender);

            modelBuilder.Entity<GameModel>().HasMany(g => g.Players).WithMany(u => u.Games);
            modelBuilder.Entity<GameModel>().HasMany(g => g.Errors).WithOne(e => e.Game);

            modelBuilder.Entity<GameCodeModel>().HasMany(g => g.Games).WithOne(g => g.GameCode);

        }


    }
}
