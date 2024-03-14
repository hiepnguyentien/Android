using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using android.Entities;
using android.Entities.Joins;

namespace android.Context;

public class ApplicationContext : IdentityDbContext<User, Role, long>
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
    }
    
    // Thực thể thuần
    public DbSet<Track> Tracks { get; set; } = null!;
    public DbSet<Playlist> Playlists { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;

    // Bảng quan hệ
    public DbSet<Follow> Follows { get; set; } = null!;
    public DbSet<Comment> Comments { get; set; } = null!;

    // Bảng liên kết n-n
    public DbSet<TrackPlaylist> TrackPlaylists { get; set; } = null!;
    public DbSet<TrackCategory> TrackCategories { get; set; } = null!;
    public DbSet<LikePlaylist> LikePlaylists { get; set; } = null!;
    public DbSet<LikeTrack> LikeTracks { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        foreach (var type in builder.Model.GetEntityTypes())
        {
            var tableName = type.GetTableName()!;
            if (tableName.StartsWith("AspNet"))
            {
                type.SetTableName(tableName.Replace("AspNet", ""));
            }
        }
    }
}