namespace android.Models.Responses;

public class UserResponseModel
{
    public long Id  { get; set; }

    public string UserName  { get; set; }  = null!;
    public string Email  { get; set; }  = null!;
    public string PhoneNumber { get; set; }  = null!;
    public string? FullName  { get; set; }  = null!;
    public string? Avatar { get; set; } = null!;
}