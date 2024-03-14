using System.ComponentModel.DataAnnotations;

namespace android.Models.Securities;

public class SigninModel
{
    [StringLength(40)]
    public string UserName { get; set; } = null!;

    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    [EmailAddress]
    public string? Email { get; set; }

    public bool RememberMe { get; set; } = false;
}