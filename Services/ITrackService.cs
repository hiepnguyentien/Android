using android.Enumerables;
using android.Models.Inserts;
using android.Models.Responses;
using android.Models.Updates;
namespace android.Services;

public interface ITrackService
{
    // Admin
    // Lấy tất cả track có phân trang
    public Task<IEnumerable<TrackResponseModel>> GetAllByAdmin();
    // Tìm kiếm các track cả public cả private
    public Task<IEnumerable<TrackResponseModel>> SearchByAdmin(string input);

    // Get By id
    public Task<TrackResponseModel> GetById(int id, ERole role, long userId);
    // User
    // Lấy tất cả track của user
    public Task<IEnumerable<TrackResponseModel>> GetAllUploadedByUser(long uid);
    // Lấy tất cả track public và của user có phân trang
    public Task<IEnumerable<TrackResponseModel>> GetAllByGuestByUser(long uid);
    // Guest
    // Lấy tất cả track public có phân trang
    public Task<IEnumerable<TrackResponseModel>> GetAllByGuest();
    // Tìm kiếm tất cả track cuả user
    public Task<IEnumerable<TrackResponseModel>> SearchByUser(string input, long userId);
    // Tìm kiếm track bởi guest
    public Task<IEnumerable<TrackResponseModel>> SearchByGuest(string input);
    
    // Nghe nhạc
    public Task<Stream> PlayTrack(string fileName, long userId);

    // Next nhạc
    public Task<string> GetNextTrack(string currentFileName);
    
    // Cập nhật thông tin track
    public Task UpdateInfomation(TrackUpdateModel model, IFormFile? fileArtwork, int trackId, long userid);
    
    // Thêm mới track
    public Task AddNew(TrackInsertModel model, long userId, IFormFile fileAudio, IFormFile? fileArtwork);
    
    // Like
    public Task LikeTrack(int userId, int trackId);
    
    // Xóa track
    public Task Remove(int id);
}