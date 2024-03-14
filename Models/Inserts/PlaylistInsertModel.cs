using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using android.Enumerables;
using android.Tools;

namespace android.Models.Inserts;

public class PlaylistInsertModel
{

    public int Id { get; set; } = 0;

    [Max(EMaxValue.NameLength_Playlist)]
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsPrivate { get; set; } = false;

    public string? Tags { get; set; }

    // Phải có ít nhất 1 bài hát mới tạo được playlist
    public IEnumerable<int> TrackIds { get; set; } = null!;
}