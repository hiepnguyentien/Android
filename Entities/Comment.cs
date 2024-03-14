using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace android.Entities;

public class Comment
{
    [Key]
    public int Id { get; set; }

    [MaxLength]
    public string Content { get; set; } = null!;

    public DateTime CommentAt { get; set; }
    
    public bool IsEdited { get; set; }
    public bool IsReported { get; set; }
    public int TrackId { get; set; }
    [DeleteBehavior(DeleteBehavior.NoAction)]
    public Track Track { get; set; } = null!;

    public long UserId { get; set; }
    [DeleteBehavior(DeleteBehavior.NoAction)]
    public User User { get; set; } = null!;
}