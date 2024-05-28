using android.Models.Inserts;
using android.Models.Responses;
using android.Models.Securities;
using android.Models.Updates;

namespace android.Services;

public interface IPlaylistService
{
    // Guest
    // Lấy theo phân trang các playlist public
    Task<IEnumerable<PlaylistResponseModel>> GetAllByGuest(int page);
    //  Lấy playlist có id = playlistId nếu được đặt public
    Task<PlaylistResponseModel?> GetViaIdByGuest(int playlistId);
    // Tìm kiếm theo tên playlist/tên người tạo/mô tả
    Task<IEnumerable<PlaylistResponseModel>> SearchByGuest(string keyword);
    Task<IEnumerable<PlaylistResponseModel>> SearchByAdmin(string keyword);


    // Admin
    // Lấy tất cả playlist của tất cả user phân trang
    Task<IEnumerable<PlaylistResponseModel>> GetAllByAdmin(int page);
    Task<PlaylistResponseModel> GetById(int trackId, long userId, bool isAdmin);

    // User
    // Lấy tất cả playlist của user đăng nhập
    Task<IEnumerable<PlaylistResponseModel>> GetAllByUser(long userId);
    // Lấy playlist trong danh sách playlist của user đăng nhập
    Task<PlaylistResponseModel?> GetViaIdByUser(int playlistId, long userId);

    // Like/Dislike 1 playlist bất kì bằng tài khoản của user đăng nhập
    Task Like(int playlistId, long userId);

    // User tạo mới 1 playlist. trả về thông báo + mã tạo mới
    Task<ResponseModel> AddNew(PlaylistInsertModel model, IFormFile? artwork, long userId);

    // User repost 1 playlist
    Task Repost(int playlistId, long userId);

    // User cập nhật thông tin 1 playlist (cả danh sách bài hát trong đó)
    Task UpdateInfomation(int playlistId, PlaylistUpdateModel model, IFormFile? artwork, long userId);
    
    // User xóa 1 playlist
    Task DeleteByCreator(int playlistId, long userId);
    Task DeleteByAdmin(int playlistId);
}