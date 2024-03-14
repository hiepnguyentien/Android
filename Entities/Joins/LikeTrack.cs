using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace android.Entities.Joins;
[Table("LikeTrack")]
public class LikeTrack
{
    [Key]
    public int Id { get; set; }

    public long UserId { get; set; }
    [ForeignKey("UserId")]
    [DeleteBehavior(DeleteBehavior.NoAction)]
    public User User { get; set; } = null!;

    public int TrackId { get; set; }
    [ForeignKey("TrackId")]
    [DeleteBehavior(DeleteBehavior.NoAction)]
    public Track Track { get; set; } = null!;
}
