using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebsiteBanHang.Models;

namespace WebsiteBanHang.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>().HasData(

    new Product { Id = 1, Name = "Clean Code", Price = 200000, CategoryId = 2, ImageUrl = "https://picsum.photos/200?1" },
    new Product { Id = 2, Name = "Atomic Habits", Price = 180000, CategoryId = 4, ImageUrl = "https://picsum.photos/200?2" },
    new Product { Id = 3, Name = "Deep Work", Price = 220000, CategoryId = 4, ImageUrl = "https://picsum.photos/200?3" },
    new Product { Id = 4, Name = "Think and Grow Rich", Price = 150000, CategoryId = 3, ImageUrl = "https://picsum.photos/200?4" },
    new Product { Id = 5, Name = "The Alchemist", Price = 130000, CategoryId = 5, ImageUrl = "https://picsum.photos/200?5" }
);
        }
    }
}