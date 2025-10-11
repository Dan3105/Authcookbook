using AuthCookbook.Core.Models;
using AuthCookbook.Core.Plugins.HashPassword;
using Microsoft.EntityFrameworkCore;

namespace AuthCookbook.Core.Plugins
{
    public class InMemoryDbContext : DbContext
    {
        private readonly IHashPasswordService _hashPasswordService;
        public InMemoryDbContext(DbContextOptions<InMemoryDbContext> options, IHashPasswordService hashPasswordService) : base(options)
        {
            _hashPasswordService = hashPasswordService;
        }

        public DbSet<UserIdentity> UserIdentities { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserIdentity>()
                .Property(u => u.Id)
                .ValueGeneratedOnAdd();
        }

        public void SeedData()
        {
            Database.EnsureCreated();
            if (!UserIdentities.Any(u => u.Username == "su"))
            {
                UserIdentities.Add(new UserIdentity()
                {
                    Id = Guid.Parse("b9e381cf-0a43-4d2e-8545-a4505996f77a"),
                    Email = "admin@tetst.com",
                    Username = "su",
                    HashPassword = _hashPasswordService.HashPassword("gw"),
                });
            }
            SaveChanges();
        }
    }
}
