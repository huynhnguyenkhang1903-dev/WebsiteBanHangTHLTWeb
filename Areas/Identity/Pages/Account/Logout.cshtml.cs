using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using WebsiteBanHang.Models;

namespace WebsiteBanHang.Areas.Identity.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public LogoutModel(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }

        // Khi load trang Logout (GET)
        public void OnGet()
        {
        }

        // Khi bấm Logout (POST)
        public async Task<IActionResult> OnPost(string? returnUrl = null)
        {
            await _signInManager.SignOutAsync();

            // 👉 Luôn chuyển về trang Login
            return RedirectToPage("/Account/Login");
        }
    }
}