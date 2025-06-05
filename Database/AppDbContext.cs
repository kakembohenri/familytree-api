using familytree_api.Models;
using Microsoft.EntityFrameworkCore;

namespace familytree_api.Database
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Family> Family { get; set; }
        public DbSet<FamilyMember> FamilyMember { get; set; }
        public DbSet<Token> Token { get; set; }
        public DbSet<Partner> Partner { get; set; } 
        public DbSet<Image> Image { get; set; } 
    }
}
