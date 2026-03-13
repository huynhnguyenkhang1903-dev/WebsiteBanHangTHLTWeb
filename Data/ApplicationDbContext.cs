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

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Sách Công Nghệ" },
                new Category { Id = 2, Name = "Sách Lập Trình" },
                new Category { Id = 3, Name = "Sách Kinh Tế" },
                new Category { Id = 4, Name = "Sách Kỹ Năng Sống" },
                new Category { Id = 5, Name = "Tiểu Thuyết" }
            );
        }
    }
}