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

            var user = await _userManager.FindByEmailAsync(Input.Email);

            // 🔥 FIX NULL (QUAN TRỌNG NHẤT)
            if (user == null)
            {
                ErrorMessage = "Email không tồn tại!";
                return Page();
            }

            // 🔥 CHECK USERNAME
            if (string.IsNullOrEmpty(user.UserName))
            {
                ErrorMessage = "Tài khoản không hợp lệ (thiếu username)";
                return Page();
            }

            var result = await _signInManager.PasswordSignInAsync(
                user.UserName,
                Input.Password,
                false,
                false
            );

            if (result.Succeeded)
            {
                // 👑 ADMIN
                if (await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    return RedirectToAction("Index", "Admin", new { area = "Admin" });
                }

                // 👤 USER
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            ErrorMessage = "Sai mật khẩu!";
            return Page();
        }
    }
}