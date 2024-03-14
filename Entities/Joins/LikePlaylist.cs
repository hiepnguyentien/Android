using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace android.Entities.Joins;

[Table("LikePlaylist")]
public class LikePlaylist
{    
    [Key]
    public int Id { get; set; }

    public long UserId { get; set; }
    [ForeignKey("UserId")]
    [DeleteBehavior(DeleteBehavior.NoAction)]
    public User User { get; set; } = null!;

    public int PlaylistId { get; set; }
    [ForeignKey("PlaylistId")]
    [DeleteBehavior(DeleteBehavior.NoAction)]
    public Playlist Playlist { get; set; } = null!;
}
