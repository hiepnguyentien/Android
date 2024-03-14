using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using android.Enumerables;
using android.Tools;

namespace android.Entities;
public class User : IdentityUser<long>
{
    public override long Id { get; set; }

    [Required]
    public override string? UserName  { get; set; }     
    [Required]
    public override string? Email { get; set ; } 

    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    [Max(EMaxValue.DirectoryLength)]
    public string? Avatar { get; set; }
}