using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebsiteBanHang.Models;

namespace WebsiteBanHang.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public LoginModel(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();

        public string ErrorMessage { get; set; } = "";

        public class InputModel
        {
            public string Email { get; set; } = "";
            public string Password { get; set; } = "";
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            var result = await _signInManager.PasswordSignInAsync(
                Input.Email,
                Input.Password,
                false,
                false);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);

                // 👑 ADMIN
                if (user != null && await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    return RedirectToAction("Index", "Admin", new { area = "Admin" });
                }

                // 👤 USER
                return RedirectToAction("Index", "Home"); // ✅ CHUẨN
            }

            ErrorMessage = "Sai email hoặc mật khẩu!";
            return Page();
        }
    }
}