namespace android.Models.Updates;

public class UserUpdateModel
{
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string? FullName { get; set; } = null!;
}