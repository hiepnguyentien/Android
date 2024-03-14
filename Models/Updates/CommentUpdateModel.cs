using android.Enumerables;
using android.Tools;

namespace android.Models.Updates;

public class CommentUpdateModel
{
    public int Id { get; set; }
    public string Content { get; set; } = null!;
}