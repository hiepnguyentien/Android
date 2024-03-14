using android.Entities;
using android.Models;
using android.Models.Inserts;
using android.Models.Responses;
using android.Models.Updates;
namespace android.Services;

public interface ICommentService
{
    //Guest
    //Xem tất cả comment của 1 track
    Task<IEnumerable<CommentResponseModel>> GetByTrackId(int trackId);

    //Admin
    //Xem tất cả comment ủa app (để kiểm tra xem có comment vi phạm hay không)
    Task<IEnumerable<CommentResponseModel>> GetAll();
    //Xem các bình luận không vi phạm
    Task<IEnumerable<CommentResponseModel>> GetNonViolationComment();
    //Xem các bình luận vi phạm
    Task<IEnumerable<CommentResponseModel>> GetViolationComment();
    //Xóa comment của người dùng bởi admin
    Task DeleteCommentByAdmin(int commentId);
    //Bỏ report comment
    Task UnReportComment(int commentId);
    //Tìm kiếm comment theo nội dung
    Task<IEnumerable<CommentResponseModel>> SearchComment(string query);
    //Tìm kiếm comment theo người dùng
    Task<IEnumerable<CommentResponseModel>> SearchCommentByUser(string query);
    

    //Member
    //Báo cáo vi phạm comment
    Task ReportComment(int commentId, long userid);
    //Xóa comment của mình
    Task DeleteCommentByCreator(int commentId, long userid);
    //Sửa comment của mình
    Task UpdateCommentByCreator(int commentId, long userid, CommentUpdateModel model);
    //Thêm comment vào track
    Task Comment(CommentInsertModel model, long userid, int trackId);
}