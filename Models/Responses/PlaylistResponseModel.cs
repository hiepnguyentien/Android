namespace android.Models.Responses;

public class PlaylistResponseModel
{
    public int Id { get; set; }
    public string PlaylistName { get; set; } = null!;

    public string ArtWork { get; set; } = null!;
    
    public long AuthorId { get; set; }
    public string AuthorName { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
    public bool IsPrivate { get; set; } = false;
    public int LikeCount { get; set; } = 0;
    public int RepostCount { get; set; } = 0;
    public string? Description { get; set; }
    public string[]? Tags { get; set; }
    public IEnumerable<int>? TrackIds { get; set; }
}