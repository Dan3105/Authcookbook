using AuthServer.Model.UserModel;
using AuthServer.Services.Shared;
using Microsoft.EntityFrameworkCore;

namespace AuthCookbook.Core.Shared.Plugins
{
    public class InMemoryDbContext : DbContext
    {
        public InMemoryDbContext(DbContextOptions<InMemoryDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>()
                .Property(u => u.id)
                .ValueGeneratedOnAdd();
        }

        public void SeedData()
        {
            Database.EnsureCreated();
            if (!Users.Any(u => u.username == "su"))
            {
                Users.Add(new User()
                {
                    id = 1,
                    email = "admin@tetst.com",
                    displayName = "su",
                    username = "su",
                    hashPassword = HashPasswordUtility.HashPassword("gw"),
                });
            }
            SaveChanges();
        }
    }
}
