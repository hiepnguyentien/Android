using System.ComponentModel.DataAnnotations;
using android.Enumerables;
using android.Entities.Joins;
using android.Tools;

namespace android.Entities;

public class Category
{
    [Key]
    public int Id { get; set; }

    [Max(EMaxValue.NameLength_Category)]
    public string Name { get; set; } = null!;
    
    [MaxLength]
    public string? Description { get; set; }
}