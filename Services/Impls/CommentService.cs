using System.Net;
using Microsoft.EntityFrameworkCore;
using android.Context;
using android.Entities;
using android.Entities.Joins;
using android.Exceptions;
using android.Models.Inserts;
using android.Models.Responses;
using android.Models.Updates;
using android.Tools;

namespace android.Services.Impls;

public class CommentService : ICommentService
{
    private readonly ApplicationContext _context;

    public CommentService(ApplicationContext context)
    {
        _context = context;
    }

    public async Task DeleteCommentByAdmin(int commentId)
    {
        var comment = await _context.Comments.FindAsync(commentId)
            ?? throw new AppException(HttpStatusCode.NotFound,
                "Không tìm thấy comment này");
        _context.Comments.Remove(comment);
        _context.Tracks.FirstOrDefault(t => t.Id == comment.TrackId)!.CommentCount--;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCommentByCreator(int commentId, long userid)
    {
        var comment =( from c in _context.Comments
                      join t in _context.Tracks
                          on c.TrackId equals t.Id
                      where c.Id == commentId && c.UserId == userid
                      select c).FirstOrDefault();
        if (comment == null)
        {
            throw new AppException(HttpStatusCode.NotFound,
                "Không tìm thấy comment này");
        }
        if (comment.UserId != userid )
        {
            throw new AppException(HttpStatusCode.Forbidden,
                "Bạn không có quyền xóa comment này");
        }
        _context.Comments.Remove(comment);
        _context.Tracks.FirstOrDefault(t => t.Id == comment.TrackId)!.CommentCount--;
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<CommentResponseModel>> GetAll()
    {
        var result = from comment in _context.Comments
                     join user in _context.Users
                         on comment.UserId equals user.Id
                     join track in _context.Tracks
                         on comment.TrackId equals track.Id
                     select new CommentResponseModel
                     {
                         Id = comment.Id,
                         Content = comment.Content,
                         CreatedAt = comment.CommentAt,
                         IsEdited = comment.IsEdited,
                         UserId = comment.UserId,
                         TrackId = comment.TrackId,
                         TrackName = track.Name,
                         Username = comment.User.UserName
                     };
        return await result.ToListAsync();
    }

    public async Task<IEnumerable<CommentResponseModel>> GetByTrackId(int trackId)
    {
        var result = from comment in _context.Comments
                     join user in _context.Users
                         on comment.UserId equals user.Id
                     join track in _context.Tracks
                         on comment.TrackId equals track.Id
                     where track.Id == trackId && !comment.IsReported
                     select new CommentResponseModel
                     {
                         Id = comment.Id,
                         Content = comment.Content,
                         CreatedAt = comment.CommentAt,
                         IsEdited = comment.IsEdited,
                         UserId = comment.UserId,
                         TrackId = comment.TrackId,
                         TrackName = track.Name,
                         Username = comment.User.UserName
                     };
        return await result.ToListAsync();
    }

    public async Task<IEnumerable<CommentResponseModel>> GetViolationComment()
    {
        var result = from comment in _context.Comments
                     join user in _context.Users
                         on comment.UserId equals user.Id
                     join track in _context.Tracks
                         on comment.TrackId equals track.Id
                     where comment.IsReported
                     select new CommentResponseModel
                     {
                         Id = comment.Id,
                         Content = comment.Content,
                         CreatedAt = comment.CommentAt,
                         IsEdited = comment.IsEdited,
                         UserId = comment.UserId,
                         TrackId = comment.TrackId,
                         TrackName = track.Name,
                         Username = comment.User.UserName
                     };
        return await result.ToListAsync();
    }

    public async Task ReportComment(int commentId, long userId)
    {
        var comment = await _context.Comments.FindAsync(commentId)
            ?? throw new AppException(HttpStatusCode.NotFound,
                "Không tìm thấy comment này");
        if (comment.UserId == userId)
        {
            throw new AppException(HttpStatusCode.Forbidden,
                "Bạn không thể report comment của chính mình");
        }
        comment.IsReported = true;
        await _context.SaveChangesAsync();
    }

    public async Task UnReportComment(int commentId)
    {
        var comment = await _context.Comments.FindAsync(commentId)
            ?? throw new AppException(HttpStatusCode.NotFound,
                "Không tìm thấy comment này");
        comment.IsReported = false;
        await _context.SaveChangesAsync();
    }

    public async Task UpdateCommentByCreator(int commentId, long userid, CommentUpdateModel model)
    {
        var comment = await _context.Comments.FindAsync(commentId)
            ?? throw new AppException(HttpStatusCode.NotFound,
                "Không tìm thấy comment này");
        if (comment.UserId != userid)
        {
            throw new AppException(HttpStatusCode.Forbidden,
                "Bạn không có quyền chỉnh sửa comment này");
        }

        comment.Content = model.Content;
        comment.IsEdited = true;
        await _context.SaveChangesAsync();
    }

    public async Task Comment(CommentInsertModel model, long userId, int trackId)
    {
        var track = await _context.Tracks.FindAsync(trackId)
            ?? throw new AppException(HttpStatusCode.NotFound,
                "Không tìm thấy bài hát này");
        var comment = new Comment()
        {
            Content = model.Content,
            CommentAt = DateTime.Now,
            TrackId = trackId,
            UserId = userId,
        };
        await _context.Comments.AddAsync(comment);
        track.CommentCount++;
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<CommentResponseModel>> GetNonViolationComment()
    {
        var result = from comment in _context.Comments
                     join user in _context.Users
                         on comment.UserId equals user.Id
                     join track in _context.Tracks
                         on comment.TrackId equals track.Id
                     where !comment.IsReported
                     select new CommentResponseModel
                     {
                         Id = comment.Id,
                         Content = comment.Content,
                         CreatedAt = comment.CommentAt,
                         IsEdited = comment.IsEdited,
                         UserId = comment.UserId,
                         TrackId = comment.TrackId,
                         TrackName = track.Name,
                         Username = comment.User.UserName
                     };
        return await result.ToListAsync();
    }

    public async Task<IEnumerable<CommentResponseModel>> SearchComment(string query)
    {
        var result = from comment in _context.Comments
                     join user in _context.Users
                         on comment.UserId equals user.Id
                     join track in _context.Tracks
                         on comment.TrackId equals track.Id
                     where comment.Content.Contains(query)
                     select new CommentResponseModel
                     {
                         Id = comment.Id,
                         Content = comment.Content,
                         CreatedAt = comment.CommentAt,
                         IsEdited = comment.IsEdited,
                         UserId = comment.UserId,
                         TrackId = comment.TrackId,
                         TrackName = track.Name,
                         Username = comment.User.UserName
                     };
        return await result.ToListAsync();
    }

    public async Task<IEnumerable<CommentResponseModel>> SearchCommentByUser(string query)
    {
        var result = from comment in _context.Comments
                     join user in _context.Users
                         on comment.UserId equals user.Id
                     join track in _context.Tracks
                         on comment.TrackId equals track.Id
                     where user.UserName.Contains(query)
                     select new CommentResponseModel
                     {
                         Id = comment.Id,
                         Content = comment.Content,
                         CreatedAt = comment.CommentAt,
                         IsEdited = comment.IsEdited,
                         UserId = comment.UserId,
                         TrackId = comment.TrackId,
                         TrackName = track.Name,
                         Username = comment.User.UserName
                     };
        return await result.ToListAsync();
    }
}
