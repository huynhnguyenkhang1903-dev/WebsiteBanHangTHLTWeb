using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

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

    // 👉 THÊM MỚI
    public string? Address { get; set; }

    [Range(1, 120, ErrorMessage = "Tuổi không hợp lệ")]
    public int? Age { get; set; }

    // 👉 ROLE (OPTIONAL)
    public string? Role { get; set; }

    public IEnumerable<SelectListItem>? RoleList { get; set; }
}