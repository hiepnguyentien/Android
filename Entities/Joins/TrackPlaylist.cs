using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace android.Entities.Joins;

[Table("Tracks_Playlists")]
[PrimaryKey("PlaylistId", "TrackId")]
public class TrackPlaylist
{
    public int TrackId { get; set; }
    [DeleteBehavior(DeleteBehavior.NoAction)]
    public Track Track { get; set; } = null!;

    public int PlaylistId { get; set; }
    [DeleteBehavior(DeleteBehavior.NoAction)]
    public Playlist Playlist { get; set; } = null!;
}