using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebsiteBanHang.Data;
using WebsiteBanHang.Models;
using WebsiteBanHang.Repositories;
using WebsiteBanHang.Extensions; // ✅ THÊM DÒNG NÀY

[Authorize]
public class ShoppingCartController : Controller
{
    private readonly IProductRepository _productRepository;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public ShoppingCartController(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        IProductRepository productRepository)
    {
        _productRepository = productRepository;
        _context = context;
        _userManager = userManager;
    }

    // ================== GET CHECKOUT ==================
    public IActionResult Checkout()
    {
        return View(new Order());
    }

    // ================== POST CHECKOUT ==================
    [HttpPost]
    public async Task<IActionResult> Checkout(Order order)
    {
        // ✅ FIX: dùng đúng extension bạn đã tạo
        var cart = HttpContext.Session.GetObject<ShoppingCart>("Cart") ?? new ShoppingCart();

        // ✅ Check giỏ hàng rỗng
        if (cart.Items == null || !cart.Items.Any())
        {
            return RedirectToAction("Index", "Cart");
        }

        // ✅ Lấy user (tránh null)
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }

        // ✅ Gán dữ liệu Order
        order.UserId = user.Id;
        order.OrderDate = DateTime.UtcNow;
        order.TotalPrice = cart.Items.Sum(i => i.Price * i.Quantity);

        // ✅ Tạo OrderDetails
        order.OrderDetails = cart.Items.Select(i => new OrderDetail
        {
            ProductId = i.ProductId,
            Quantity = i.Quantity,
            Price = i.Price
        }).ToList();

        // ✅ Lưu DB
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        // ✅ Xóa giỏ hàng
        HttpContext.Session.Remove("Cart");

        // ✅ Trang hoàn tất
        return View("OrderCompleted", order.Id);
    }
}