using android.Enumerables;
using android.Tools;

namespace android.Models.Responses;
public class CommentResponseModel
{
    public int Id { get; set; }
    public string Content { get; set; } = null!;
    public long UserId { get; set; }
    public string? Username { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public bool IsEdited { get; set; }
    public int TrackId { get; set; }
    public string TrackName { get; set; } = null!;
    public bool IsReported { get; set; }
    public string? Avatar { get; set; }
}