using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using WebsiteBanHang.Models;

namespace WebsiteBanHang.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public RegisterModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();

        public List<string> Errors { get; set; } = new List<string>();

        public string? SuccessMessage { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Họ tên không được để trống")]
            public string FullName { get; set; } = string.Empty;

            [Required(ErrorMessage = "Email không được để trống")]
            [EmailAddress(ErrorMessage = "Email không hợp lệ")]
            public string Email { get; set; } = string.Empty;

            [Required(ErrorMessage = "Mật khẩu không được để trống")]
            [DataType(DataType.Password)]
            [MinLength(6, ErrorMessage = "Mật khẩu phải từ 6 ký tự")]
            public string Password { get; set; } = string.Empty;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Kiểm tra email đã tồn tại
            var existingUser = await _userManager.FindByEmailAsync(Input.Email);

            if (existingUser != null)
            {
                Errors.Add("Email đã tồn tại. Vui lòng sử dụng email khác!");
                return Page();
            }

            var user = new ApplicationUser
            {
                UserName = Input.Email,
                Email = Input.Email,
                FullName = Input.FullName
            };

            var result = await _userManager.CreateAsync(user, Input.Password);

            if (result.Succeeded)
            {
                SuccessMessage = "Tạo tài khoản thành công! Bạn có thể đăng nhập ngay.";

                // reset form
                Input = new InputModel();

                return Page();
            }

            foreach (var error in result.Errors)
            {
                Errors.Add(error.Description);
            }

            return Page();
        }
    }
}
