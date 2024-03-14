using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace android.Entities.Joins;

[PrimaryKey("FollowedUserId", "FollowingUserId")]
public class Follow
{
    public long FollowingUserId { get; set; }
    [DeleteBehavior(DeleteBehavior.NoAction)]
    public User FollowingUser { get; set; } = null!;

    public long FollowedUserId { get; set; }
    [DeleteBehavior(DeleteBehavior.NoAction)]
    public User FollowedUser { get; set; } = null!;

    [DataType(DataType.DateTime)]
    public DateTime FollowedAt { get; set; }
}