using Microsoft.AspNetCore.Mvc;
using WebsiteBanHang.Models;
using WebsiteBanHang.Repositories;
using WebsiteBanHang.Extensions;

namespace WebsiteBanHang.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductRepository _productRepository;

        public CartController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // ================== GET CART ==================
        private ShoppingCart GetCart()
        {
            return HttpContext.Session.GetObject<ShoppingCart>("Cart") ?? new ShoppingCart();
        }

        private void SaveCart(ShoppingCart cart)
        {
            HttpContext.Session.SetObject("Cart", cart);
        }

        // ================== ADD ==================
        public async Task<IActionResult> AddToCart(int productId)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null) return NotFound();

            var cart = GetCart();

            cart.AddItem(new CartItem
            {
                ProductId = product.Id,
                Name = product.Name ?? "",
                Price = product.Price,
                Quantity = 1
            });

            SaveCart(cart);
            return RedirectToAction("Index");
        }

        // ================== VIEW ==================
        public IActionResult Index()
        {
            return View(GetCart());
        }

        // ================== INCREASE ==================
        public IActionResult Increase(int id)
        {
            var cart = GetCart();
            var item = cart.Items.FirstOrDefault(x => x.ProductId == id);

            if (item != null)
                item.Quantity++;

            SaveCart(cart);
            return RedirectToAction("Index");
        }

        // ================== DECREASE ==================
        public IActionResult Decrease(int id)
        {
            var cart = GetCart();
            var item = cart.Items.FirstOrDefault(x => x.ProductId == id);

            if (item != null && item.Quantity > 1)
                item.Quantity--;

            SaveCart(cart);
            return RedirectToAction("Index");
        }

        // ================== REMOVE ==================
        public IActionResult Remove(int id)
        {
            var cart = GetCart();
            cart.Items.RemoveAll(x => x.ProductId == id);

            SaveCart(cart);
            return RedirectToAction("Index");
        }

        // ================== CHECKOUT ==================
        [HttpPost]
        public IActionResult Checkout()
        {
            var cart = GetCart();

            if (cart.Items == null || !cart.Items.Any())
            {
                return RedirectToAction("Index");
            }

            // 👉 Ở đây bạn có thể chuyển sang trang nhập thông tin
            return RedirectToAction("Checkout", "ShoppingCart");
        }
    }
}