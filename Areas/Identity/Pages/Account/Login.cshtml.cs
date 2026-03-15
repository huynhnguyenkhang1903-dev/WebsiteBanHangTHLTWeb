using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebsiteBanHang.Models;

namespace WebsiteBanHang.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public LoginModel(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();

        public string ErrorMessage { get; set; } = "";

        public class InputModel
        {
            public string Email { get; set; } = "";
            public string Password { get; set; } = "";
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var result = await _signInManager.PasswordSignInAsync(
                Input.Email,
                Input.Password,
                false,
                false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Product");
            }

            ErrorMessage = "Sai email hoặc mật khẩu!";
            return Page();
        }
    }
}