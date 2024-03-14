using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using android.Enumerables;
using android.Tools;
using Microsoft.EntityFrameworkCore;

namespace android.Entities;

[Index("Name","AuthorId", IsUnique = true)]
public class Playlist
{
    [Key]
    public int Id { get; set; }

    [Max(EMaxValue.NameLength_Playlist)]
    public string Name { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public bool IsPrivate { get; set; }

    public int LikeCount { get; set; }
    public int RepostCount { get; set; }
    public int TrackCount { get; set; }

    [Max(EMaxValue.DirectoryLength)]
    public string ArtWork { get; set; } = null!;

    public long AuthorId { get; set; }
    [ForeignKey("AuthorId")]
    public User Author { get; set; } = null!;

    [MaxLength]
    public string? Description { get; set; }

    [MaxLength]
    public string? Tags { get; set; }
}