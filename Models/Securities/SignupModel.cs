using System.ComponentModel.DataAnnotations;
using android.Enumerables;
using android.Tools;

namespace android.Models.Securities;

public class SignupModel 
{
    [Max(EMaxValue.NameLength_UserName)]
    [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
    public string UserName  { get; set; } = null!;

    [EmailAddress(ErrorMessage = "Email không hợp lệ")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Mật khẩu không được để trống")]
    public string Password { get; set; } = null!;
    
    public string? PhoneNumber { get; set; }

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}