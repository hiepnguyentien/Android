using android.Enumerables;

namespace android.Models.Securities;

public class AuthInformation
{
    public long UserId { get; set; }
    public string UserName { get; set; }  = null!;
    public string Email { get; set; }  = null!;
    public ERole Role { get; set; } = ERole.GUEST;
}