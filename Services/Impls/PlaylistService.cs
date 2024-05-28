using Microsoft.EntityFrameworkCore;
using System.Net;
using android.Context;
using android.Entities;
using android.Entities.Joins;
using android.Exceptions;
using android.Models.Inserts;
using android.Models.Responses;
using android.Models.Securities;
using android.Models.Updates;
using android.Tools;

namespace android.Services.Impl;

public class PlaylistService : IPlaylistService
{
    private readonly ApplicationContext _context;

    private readonly int PAGE_SIZE = 10;
    public PlaylistService(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PlaylistResponseModel>> GetAllByGuest()
    {
        return await _context.Playlists
            .OrderBy(p => p.Id)
            .Where(p => !p.IsPrivate)
            .Select(p => new PlaylistResponseModel
            {
                Id = p.Id,
                PlaylistName = p.Name,
                AuthorName = p.Author.UserName!,
                CreatedAt = p.CreatedAt,
                Description = p.Description,            
                ArtWork = p.ArtWork,
                Tags = TagTool.GetTags(p.Tags),
            })
            .ToListAsync();
    }

    public async Task<PlaylistResponseModel?> GetViaIdByGuest(int playlistId)
    {
        var playlist = await _context.Playlists
                .Include(p => p.Author)
                .FirstOrDefaultAsync(p => p.Id == playlistId)
            ?? throw new AppException(HttpStatusCode.NotFound,
                "Không tìm thấy playlist yêu cầu");
        if (playlist.IsPrivate)
            throw new AppException(HttpStatusCode.Forbidden,
                "Chủ sở hữu đã tắt hiển thị playlist này");
        return new PlaylistResponseModel
        {
            Id = playlistId,
            AuthorId = playlist.AuthorId,
            PlaylistName = playlist.Name,
            AuthorName = UserTool.GetAuthorName(playlist.Author),
            CreatedAt = playlist.CreatedAt,
            Description = playlist.Description,
            IsPrivate = playlist.IsPrivate,
            LikeCount = playlist.LikeCount,
            RepostCount = playlist.RepostCount,
            ArtWork = playlist.ArtWork,
            Tags = TagTool.GetTags(playlist.Tags),
            TrackIds = _context.TrackPlaylists
                    .Where(tp => tp.PlaylistId == playlistId)
                    .Select(tp => tp.TrackId)
                    .ToList()
        };
    }

    public async Task<IEnumerable<PlaylistResponseModel>> SearchByAdmin(string keyword)
    {
        return await _context.Playlists
            .Where(p => p.Name.Contains(keyword)
                    || (p.Tags != null && p.Tags.Contains(keyword)) 
                    || $"{p.Author.FirstName} {p.Author.LastName} {p.Author.UserName}".Contains(keyword)
                    || (p.Description != null && p.Description.Contains(keyword)))
            .Select(p => new PlaylistResponseModel
            {
                Id = p.Id,
                PlaylistName = p.Name,
                AuthorName = UserTool.GetAuthorName(p.Author),
                CreatedAt = p.CreatedAt,
                Description = p.Description,
                ArtWork = p.ArtWork,
                Tags = TagTool.GetTags(p.Tags),
                IsPrivate = p.IsPrivate
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<PlaylistResponseModel>> SearchByGuest(string keyword)
    {
        return await _context.Playlists
            .Where(p => !p.IsPrivate && (p.Name.Contains(keyword)
                    || (p.Tags != null && p.Tags.Contains(keyword))
                    || (p.Description != null && p.Description.Contains(keyword))))
            .Select(p => new PlaylistResponseModel
            {
                Id = p.Id,
                PlaylistName = p.Name,
                AuthorName = UserTool.GetAuthorName(p.Author),
                CreatedAt = p.CreatedAt,
                Description = p.Description,
                ArtWork = p.ArtWork,
                Tags = TagTool.GetTags(p.Tags),
                IsPrivate = false
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<PlaylistResponseModel>> GetAllByAdmin()
    {
        return await _context.Playlists
            .OrderBy(p => p.Id)
            .Select(p => new PlaylistResponseModel
            {
                Id = p.Id,
                PlaylistName = p.Name,
                AuthorId = p.AuthorId,
                AuthorName = p.Author.UserName!,
                CreatedAt = p.CreatedAt,
                Description = p.Description,
                LikeCount = p.LikeCount,
                RepostCount = p.RepostCount,
                IsPrivate = p.IsPrivate,
                ArtWork = p.ArtWork,
                Tags = TagTool.GetTags(p.Tags),
            })
            .ToListAsync();
    }  

    public async Task<PlaylistResponseModel> GetById(int playlistId, long userId, bool isAdmin) {
        var p = await _context.Playlists.FindAsync(playlistId)!;
        if (userId != p.Id)
        {
            throw new AppException(HttpStatusCode.NotFound, 
                    "Không tìm thấy playlist yêu cầu");
        }
        return new PlaylistResponseModel
                    {
                        Id = p.Id,
                        AuthorId = p.AuthorId,
                        PlaylistName = p.Name,
                        AuthorName = p.Author.UserName!,
                        CreatedAt = p.CreatedAt,
                        LikeCount = p.LikeCount,
                        RepostCount = p.RepostCount,
                        Description = p.Description,
                        ArtWork = p.ArtWork,
                        Tags = TagTool.GetTags(p.Tags),
                        IsPrivate = p.IsPrivate
                    };
    }

    public async Task<IEnumerable<PlaylistResponseModel>> GetAllByUser(long userId)
    {
        return await _context.Playlists
            .Where(p => p.AuthorId == userId)
            .Select(p => new PlaylistResponseModel
            {
                Id = p.Id,
                AuthorId = p.AuthorId,
                PlaylistName = p.Name,
                AuthorName = p.Author.UserName!,
                CreatedAt = p.CreatedAt,
                LikeCount = p.LikeCount,
                RepostCount = p.RepostCount,
                Description = p.Description,
                ArtWork = p.ArtWork,
                Tags = TagTool.GetTags(p.Tags),
                IsPrivate = p.IsPrivate
            })
            .ToListAsync();
    }

    public async Task<PlaylistResponseModel?> GetViaIdByUser(int playlistId, long userId)
    {
        var result = await _context.Playlists
            .Where(p => p.AuthorId == userId && p.Id == playlistId)
            .FirstOrDefaultAsync()
                ?? throw new AppException(HttpStatusCode.NotFound, 
                    "Không tìm thấy playlist yêu cầu");
        return new PlaylistResponseModel
            {
                Id = result.Id,
                PlaylistName = result.Name,
                AuthorName = UserTool.GetAuthorName(result.Author),
                CreatedAt = result.CreatedAt,
                AuthorId = result.AuthorId,
                LikeCount = result.LikeCount,
                IsPrivate = result.IsPrivate,
                RepostCount = result.RepostCount,
                Description = result.Description,
                ArtWork = result.ArtWork,
                Tags = TagTool.GetTags(result.Tags),
                TrackIds = await _context.TrackPlaylists
                    .Where(tp => tp.PlaylistId == playlistId)
                    .Select(tp => tp.TrackId)
                    .ToListAsync()
        };  
    }

    public async Task Like(int playlistId, long userId)
    {
        var playlist = await _context.Playlists.FindAsync(playlistId) 
            ?? throw new AppException(HttpStatusCode.NotFound, 
                "Không tìm thấy playlist yêu cầu");
        if (playlist.IsPrivate)
            throw new AppException(HttpStatusCode.Forbidden, 
                "Playlist này đã bị chủ sở hữu tắt hiển thị");
        var like = await _context.LikePlaylists
            .FirstOrDefaultAsync(l => l.UserId == userId && l.PlaylistId == playlistId);
        if (like == null)
        {
            playlist.LikeCount++;
            await _context.LikePlaylists.AddAsync(new LikePlaylist
            {
                PlaylistId = playlistId,
                UserId = userId,
            });
        }
        else
        {
            playlist.LikeCount--;
            if (playlist.LikeCount < 0) 
                playlist.LikeCount = 0;
            _context.LikePlaylists.Remove(like);
        }
        await _context.SaveChangesAsync();
    }

    public async Task<ResponseModel> AddNew(PlaylistInsertModel model, IFormFile? artwork, long userId)
    {
        if (string.IsNullOrEmpty(model.Name)) 
            throw new AppException(HttpStatusCode.BadRequest, 
                "Tên playlist không được để trống");
        if (model.TrackIds == null || !model.TrackIds.Any())
            throw new AppException(HttpStatusCode.BadRequest,
                "Phải có ít nhất 1 track để tạo được playlist");
        // Chống việc 1 user tạo playlist trùng tên
        if (_context.Playlists.Any(p => p.AuthorId == userId && p.Name == model.Name))
            throw new AppException(HttpStatusCode.BadRequest, 
                $"Đã tồn tại playlist tên {model.Name}");
        var playlist = new Playlist
        {
            Name = model.Name,
            Description = model.Description,
            IsPrivate = model.IsPrivate,
            Tags = TagTool.SetTags(model.Tags),
            AuthorId = userId,
            CreatedAt = DateTime.UtcNow,
        };
        if (artwork != null)
        {
            await FileTool.SaveArtwork(artwork);
            playlist.ArtWork = artwork.FileName;
        } else 
        {
            playlist.ArtWork = "default.png";
        }
        await _context.Playlists.AddAsync(playlist);
        await _context.SaveChangesAsync();
        return new ResponseModel
        {
            IsSucceed = true,
            Data = $"Tạo thành công playlist mã : {playlist.Id}"
        };
    }

    public async Task Repost(int playlistId, long userId)
    {
        var playlist = await _context.Playlists.FindAsync(playlistId)
             ?? throw new AppException(HttpStatusCode.NotFound, 
            "Không thấy playlist yêu cầu");
        playlist.RepostCount++;
        await _context.SaveChangesAsync();
        await _context.Playlists.AddAsync(new Playlist
        {
            Name = playlist.Name,
            Description = playlist.Description,
            IsPrivate = playlist.IsPrivate,
            Tags = playlist.Tags,
            AuthorId = userId,
            CreatedAt = DateTime.UtcNow,
            ArtWork = playlist.ArtWork,
            LikeCount = playlist.LikeCount,
            RepostCount = playlist.RepostCount,
        });
        await _context.SaveChangesAsync();
    }

    public async Task UpdateInfomation(int playlistId, PlaylistUpdateModel model, IFormFile? artwork, long userId)
    {
        if (await _context.Playlists.AnyAsync(p => p.Author.Id == userId && p.Name == model.Name))
        {
            throw new AppException(HttpStatusCode.BadRequest, 
                $"Bạn đã tạo playlist có tên {model.Name} rồi");
        }
        var playlist = await _context.Playlists.FindAsync(playlistId) 
            ?? throw new AppException(HttpStatusCode.NotFound, 
                "Không thấy playlist yêu cầu");
        if (playlist.AuthorId != userId)
            throw new AppException(HttpStatusCode.Forbidden, 
                "Bạn không có quyền chỉnh sửa playlist này");

        playlist.Name = model.Name;
        playlist.Description = model.Description;
        playlist.Tags = TagTool.SetTags(model.Tags);
        playlist.IsPrivate = model.IsPrivate;

        if (artwork != null)
        {
            await FileTool.SaveArtwork(artwork);
            playlist.ArtWork = artwork.FileName;
        }
        await _context.SaveChangesAsync();
    }

    public async Task DeleteByCreator(int playlistId, long userId)
    {
        var playlist = await _context.Playlists.FindAsync(playlistId) 
            ?? throw new AppException(HttpStatusCode.NotFound, 
                "Không thấy playlist yêu cầu");
        if (playlist.AuthorId != userId)
            throw new AppException(HttpStatusCode.Forbidden, 
                "Bạn không có quyền xóa playlist này");
        
        var trackPlaylists = await _context.TrackPlaylists
            .Where(tp => tp.PlaylistId == playlistId).ToListAsync();
        if (trackPlaylists != null)
            _context.TrackPlaylists.RemoveRange(trackPlaylists);
        
        var likes = await _context.LikePlaylists
            .Where(l => l.PlaylistId == playlistId).ToListAsync();
        if (likes != null)
            _context.LikePlaylists.RemoveRange(likes);
        _context.Playlists.Remove(playlist);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteByAdmin(int playlistId)
    {
        var playlist = await _context.Playlists.FindAsync(playlistId)
            ?? throw new AppException(HttpStatusCode.NotFound,
                "Không thấy playlist yêu cầu");
        
        var trackPlaylists = await _context.TrackPlaylists
            .Where(tp => tp.PlaylistId == playlistId).ToListAsync();
        if (trackPlaylists != null)
            _context.TrackPlaylists.RemoveRange(trackPlaylists);
        
        var likes = await _context.LikePlaylists
            .Where(l => l.PlaylistId == playlistId).ToListAsync();
        if (likes != null)
            _context.LikePlaylists.RemoveRange(likes);
        
        _context.Playlists.Remove(playlist);
        await _context.SaveChangesAsync();
    }

}