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
        public DbSet<GameQuestionModel> GameQuestions { get; set; }
        public DbSet<GameAction> GameActions { get; set; }
        public DbSet<ContactMessageModel> ContactMessages { get; set; }
        public DbSet<ReportModel> Reports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>().HasMany(u => u.Games).WithMany(g => g.Players);
            modelBuilder.Entity<UserModel>().HasMany(u => u.TurnsIn).WithOne(g => g.Turn);



            modelBuilder.Entity<UserModel>().HasMany(u => u.Reports).WithOne(r => r.ReportedUser);
            modelBuilder.Entity<UserModel>().HasMany(u => u.ReportsMade).WithOne(r => r.ReportingUser);
        }


    }
}
