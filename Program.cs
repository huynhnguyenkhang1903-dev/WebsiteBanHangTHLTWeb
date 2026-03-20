using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebsiteBanHang.Data;
using WebsiteBanHang.Models;
using WebsiteBanHang.Repositories;

var builder = WebApplication.CreateBuilder(args);

// ================== DATABASE ==================
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ================== IDENTITY ==================
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// ================== COOKIE ==================
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.LogoutPath = "/Identity/Account/Logout";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});

// ================== REPOSITORY ==================
builder.Services.AddScoped<IProductRepository, EFProductRepository>();
builder.Services.AddScoped<ICategoryRepository, EFCategoryRepository>();

// ================== MVC ==================
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// ================== MIDDLEWARE ==================
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// ================== ROUTE ==================

// 👑 AREA
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Admin}/{action=Index}/{id?}"
);

// 🏠 DEFAULT
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.MapRazorPages();


// ================== SEED DATA ==================
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<ApplicationDbContext>();
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    string adminEmail = "admin@gmail.com";
    string password = "Admin123!";

    // ===== ROLE =====
    if (!await roleManager.RoleExistsAsync("Admin"))
        await roleManager.CreateAsync(new IdentityRole("Admin"));

    if (!await roleManager.RoleExistsAsync("User"))
        await roleManager.CreateAsync(new IdentityRole("User"));

    // ===== ADMIN =====
    var user = await userManager.FindByEmailAsync(adminEmail);

    if (user == null)
    {
        user = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            FullName = "Admin",
            Address = "HCM",
            Age = 20
        };

        var result = await userManager.CreateAsync(user, password);

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, "Admin");
        }
        else
        {
            foreach (var error in result.Errors)
            {
                Console.WriteLine("ERROR: " + error.Description);
            }
        }
    }

    // ===== CATEGORY =====
    if (!context.Categories.Any())
    {
        context.Categories.Add(new Category
        {
            Name = "Sách tổng hợp"
        });
        context.SaveChanges();
    }

    // ===== PRODUCT (10 mẫu) =====
    if (!context.Products.Any())
    {
        var categoryId = context.Categories.First().Id;

        var products = new List<Product>();

        for (int i = 1; i <= 10; i++)
        {
            products.Add(new Product
            {
                Name = "Sách hay " + i,
                Price = 50000 + i * 1000,
                Description = "Mô tả sách số " + i,
                ImageUrl = "https://picsum.photos/300/400?random=" + i,
                CategoryId = categoryId
            });
        }

        context.Products.AddRange(products);
        context.SaveChanges();
    }
}

app.Run();