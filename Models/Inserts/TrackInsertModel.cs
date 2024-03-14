namespace android.Models.Inserts;

public class TrackInsertModel
{
    public string TrackName { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsPrivate { get; set; }
    public IEnumerable<int>? CategoryIds { get; set; }
}