using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using android.Context;
using android.Entities;
using android.Entities.Joins;
using android.Enumerables;
using android.Exceptions;
using android.Models.Inserts;
using android.Models.Responses;
using android.Models.Updates;
using android.Tools;

namespace android.Services.Impls;

public class TrackService : ITrackService
{
    private readonly ApplicationContext _context;
    private readonly int PAGE_SIZE = 10;
    public TrackService(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TrackResponseModel>> GetAllByAdmin()
    {
        return await _context.Tracks
            .Select(track => new TrackResponseModel()
                {
                    Id = track.Id,
                    TrackName = track.Name,
                    Author = UserTool.GetAuthorName(track.Author),
                    FileName = track.FileName,
                    ArtWork = track.ArtWork,
                    Description = track.Description,
                    UploadAt = track.UploadAt,
                    ListenCount = track.ListenCount,
                    LikeCount = track.LikeCount,
                    CommentCount = track.CommentCount
                })
            .ToListAsync();
    }

    public async Task<IEnumerable<TrackResponseModel>> SearchByAdmin(string input)
    {
        return await _context.Tracks
            .Where(t => t.Name.Contains(input) || t.Author.UserName!.Contains(input))
            .Select(track => new TrackResponseModel()
            {
                Id = track.Id,
                TrackName = track.Name,
                Author = UserTool.GetAuthorName(track.Author),
                FileName = track.FileName,
                ArtWork = track.ArtWork,
                Description = track.Description,
                UploadAt = track.UploadAt,
                ListenCount = track.ListenCount,
                LikeCount = track.LikeCount,
                CommentCount = track.CommentCount
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<TrackResponseModel>> GetAllByGuestByUser(long uid)
    {
        return await _context.Tracks
            .Where(t => t.AuthorId == uid || !t.IsPrivate)
            .OrderBy(t => t.Id)
            .Select(track => new TrackResponseModel()
            {
                Id = track.Id,
                TrackName = track.Name,
                Author = UserTool.GetAuthorName(track.Author),
                FileName = track.FileName,
                ArtWork = track.ArtWork,
                IsPrivate = track.IsPrivate,
                Description = track.Description,
                UploadAt = track.UploadAt,
                ListenCount = track.ListenCount,
                LikeCount = track.LikeCount,
                CommentCount = track.CommentCount
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<TrackResponseModel>> GetAllUploadedByUser(long uid)
    {
        return await _context.Tracks
            .Where(t => t.AuthorId == uid)
            .Select(track => new TrackResponseModel()
            {
                Id = track.Id,
                TrackName = track.Name,
                Author = UserTool.GetAuthorName(track.Author),
                FileName = track.FileName,
                ArtWork = track.ArtWork,
                IsPrivate = track.IsPrivate,
                Description = track.Description,
                UploadAt = track.UploadAt,
                ListenCount = track.ListenCount,
                LikeCount = track.LikeCount,
                CommentCount = track.CommentCount
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<TrackResponseModel>> GetAllByGuest()
    {
        return await _context.Tracks
            .Where(t => !t.IsPrivate)
            .OrderBy(t => t.Id)
            .Select(track => new TrackResponseModel()
            {
                Id = track.Id,
                TrackName = track.Name,
                Author = UserTool.GetAuthorName(track.Author),
                FileName = track.FileName,
                ArtWork = track.ArtWork,
                IsPrivate = track.IsPrivate,
                Description = track.Description,
                UploadAt = track.UploadAt,
                ListenCount = track.ListenCount,
                LikeCount = track.LikeCount,
                CommentCount = track.CommentCount
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<TrackResponseModel>> SearchByUser(string input, long userId)
    {
        return await _context.Tracks
            .Where(t => t.AuthorId == userId && (t.Name.Contains(input) || t.Author.UserName!.Contains(input)))
            .Select(track => new TrackResponseModel()
            {
                Id = track.Id,
                TrackName = track.Name,
                Author = UserTool.GetAuthorName(track.Author),
                FileName = track.FileName,
                ArtWork = track.ArtWork,
                Description = track.Description,
                UploadAt = track.UploadAt,
                ListenCount = track.ListenCount,
                LikeCount = track.LikeCount,
                CommentCount = track.CommentCount
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<TrackResponseModel>> SearchByGuest(string input)
    {
        return await _context.Tracks
            .Where(t => !t.IsPrivate 
                && (t.Name.Contains(input) 
                    || (t.Author.FirstName != null && t.Author.FirstName.Contains(input))
                    || (t.Author.LastName != null && t.Author.LastName.Contains(input))
                    || t.Author.UserName!.Contains(input)
                    || (t.Description != null && t.Description.Contains(input))))
            .Select(track => new TrackResponseModel()
            {
                Id = track.Id,
                TrackName = track.Name,
                Author = UserTool.GetAuthorName(track.Author),
                FileName = track.FileName,
                ArtWork = track.ArtWork,
                Description = track.Description,
                UploadAt = track.UploadAt,
                ListenCount = track.ListenCount,
                LikeCount = track.LikeCount,
                CommentCount = track.CommentCount
            })
            .ToListAsync();
    }


    public async Task<TrackResponseModel> GetById(int id, ERole role, long userId)
    {
        var track = await _context.Tracks
            .Include(t => t.Author)
            .Where(t => t.Id == id)
            .FirstOrDefaultAsync()
                ?? throw new AppException(HttpStatusCode.NotFound,
                    "Không tìm thấy bài hát");
        switch (role)
        {
            case ERole.GUEST:
                if (track.IsPrivate)
                {
                    throw new AppException(HttpStatusCode.Forbidden,
                        "Bài hát bị khóa bởi chủ sở hữu");
                }
                break;
            case ERole.USER:
                if (track.AuthorId != userId)
                {
                    throw new AppException(HttpStatusCode.Forbidden,
                        "Bạn không có quyền xem bài hát này");
                }
                break;
            case ERole.ADMIN:
            case ERole.SUPERADMIN:
                break;
        }
        return new TrackResponseModel()
        {
            Id = track.Id,
            TrackName = track.Name,
            Author = UserTool.GetAuthorName(track.Author),
            FileName = track.FileName,
            ArtWork = track.ArtWork,
            Description = track.Description,
            UploadAt = track.UploadAt,
            ListenCount = track.ListenCount,
            LikeCount = track.LikeCount,
            CommentCount = track.CommentCount
        };
    }

    public async Task UpdateInfomation(TrackUpdateModel model, IFormFile? fileArtwork, int trackId, long userid)
    {
        var currentTrack = await _context.Tracks.FindAsync(trackId)
            ?? throw new AppException(HttpStatusCode.NotFound,
                "Không tìm thấy bài hát");
        if (currentTrack.AuthorId != userid)
        {
            throw new AppException(HttpStatusCode.Forbidden,
                "Bạn không có quyền chỉnh sửa bài hát này");
        }
        currentTrack.Name = model.TrackName;
        currentTrack.Description = model.Description;
        currentTrack.IsPrivate = model.IsPrivate;
        if (fileArtwork != null && fileArtwork.FileName != currentTrack.ArtWork)
        {
            await FileTool.SaveArtwork(fileArtwork);
            currentTrack.ArtWork = fileArtwork.FileName;
        }
        var currentCategories = await _context.TrackCategories
            .Where(tc => tc.TrackId == trackId)
            .ToListAsync();
        _context.TrackCategories.RemoveRange(currentCategories);
        if (model.CategoryIds != null)
        {
            foreach (var cid in model.CategoryIds)
            {
                if (_context.Categories.Any(p => p.Id == cid))
                {
                    await _context.TrackCategories
                        .AddAsync(new TrackCategory() 
                            { 
                                CategoryId = cid,
                                TrackId = trackId 
                            });
                }
            }
        }
        await _context.SaveChangesAsync();
    }

    public async Task AddNew(TrackInsertModel model, long userId, IFormFile fileTrack, IFormFile? fileArtwork)
    {
        var track = new Track()
        {
            Name = model.TrackName,
            Description = model.Description,
            FileName = fileTrack.FileName,
            AuthorId = userId,
            UploadAt = DateTime.Now,
            IsPrivate = model.IsPrivate,
        };
        await FileTool.SaveTrack(fileTrack);
        if (fileArtwork != null)
        {
            await FileTool.SaveArtwork(fileArtwork);
            track.ArtWork = fileArtwork.FileName;
        }
        else
        {
            track.ArtWork = FileTool.DefaultArtWork;
        }
        await _context.Tracks.AddAsync(track);
        await _context.SaveChangesAsync();
        if (model.CategoryIds != null)
        {
            foreach (var item in model.CategoryIds)
            {
                if (_context.Categories.Any(p => p.Id == item))
                {
                    await _context.TrackCategories.AddAsync(new TrackCategory() 
                        { 
                            CategoryId = item,
                            TrackId = track.Id
                        });
                }
            }
            await _context.SaveChangesAsync();
        }
    }

    public async Task UpdateInfomation(TrackUpdateModel model, IFormFile? fileArtwork, int trackId)
    {
        var currentTrack = await _context.Tracks.FindAsync(trackId)
            ?? throw new AppException(HttpStatusCode.NotFound,
                "Không tìm thấy bài hát");

        currentTrack.Name = model.TrackName;
        currentTrack.Description = model.Description;
        if (fileArtwork != null && fileArtwork.FileName != currentTrack.ArtWork)
        {
            await FileTool.SaveArtwork(fileArtwork);
            currentTrack.ArtWork = fileArtwork.FileName;
        }
        var currentCategories = await _context.TrackCategories
            .Where(tc => tc.TrackId == trackId)
            .ToListAsync();
        _context.TrackCategories.RemoveRange(currentCategories);
        await _context.SaveChangesAsync();
    }

    public async Task Remove(int id)
    {
        var track = await _context.Tracks.FindAsync(id)
            ?? throw new AppException(HttpStatusCode.NotFound,
                "Không tìm thấy bài hát");
        _context.Tracks.Remove(track);
        await _context.SaveChangesAsync();
    } 

    public async Task LikeTrack(int userId, int trackId)
    {
        var like = await _context.LikeTracks
            .FirstOrDefaultAsync(l => l.UserId == userId && l.TrackId == trackId);
        if (like == null)
        {
            await _context.LikeTracks.AddAsync(new LikeTrack()
            {
                UserId = userId,
                TrackId = trackId,
            });
        }
        else
        {
            _context.LikeTracks.Remove(like);
        }
        await _context.SaveChangesAsync();
    }

    public async Task<Stream> PlayTrack(string fileName, long userId)
    {
        var track = await _context.Tracks
            .FirstOrDefaultAsync(t => t.FileName == fileName) ?? throw new AppException(HttpStatusCode.NotFound,
                "Không tìm thấy bài hát");
        if (track.IsPrivate && track.AuthorId != userId)
        {
            throw new AppException(HttpStatusCode.Forbidden,
                "Bài hát bị khóa bởi chủ sở hữu");
        }
        return FileTool.ReadTrack(fileName);
    }

    public Task<string> GetNextTrack(string currentFileName)
    {
        var nextTrack = _context.Tracks
            .Where(t => t.FileName != currentFileName)
            .OrderBy(t => t.Id)
            .FirstOrDefault();
        return Task.FromResult(nextTrack?.FileName);
    }

        public Task<IEnumerable<TrackResponseModel>> PlaylistByOrder(int playlistId)
    {
        var tracks = _context.TrackPlaylists
            .Where(tp => tp.PlaylistId == playlistId)
            .OrderBy(tp => tp.TrackId)
            .Select(tp => new TrackResponseModel
            {
                Id = tp.TrackId,
                TrackName = tp.Track.Name,
                Author = tp.Track.Author.UserName!,
                ArtWork = tp.Track.ArtWork,
                UploadAt = tp.Track.UploadAt,
                LikeCount = tp.Track.LikeCount,
                FileName = tp.Track.FileName,
                IsPrivate = tp.Track.IsPrivate,
                ListenCount = tp.Track.ListenCount,
                CommentCount = tp.Track.CommentCount,
                Description = tp.Track.Description
            });
        return Task.FromResult(tracks.AsEnumerable());
    }

    public Task<IEnumerable<TrackResponseModel>> PlaylistRandomly(int playlistId)
    {
        var tracks = _context.TrackPlaylists
            .Where(tp => tp.PlaylistId == playlistId)
            .OrderBy(tp => Guid.NewGuid())
            .Select(tp => new TrackResponseModel
            {
                Id = tp.TrackId,
                TrackName = tp.Track.Name,
                Author = tp.Track.Author.UserName!,
                ArtWork = tp.Track.ArtWork,
                UploadAt = tp.Track.UploadAt,
                LikeCount = tp.Track.LikeCount,
                FileName = tp.Track.FileName,
                IsPrivate = tp.Track.IsPrivate,
                ListenCount = tp.Track.ListenCount,
                CommentCount = tp.Track.CommentCount,
                Description = tp.Track.Description
            });
        return Task.FromResult(tracks.AsEnumerable());
    }
}