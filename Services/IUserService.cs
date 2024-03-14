using android.Models.Responses;
using android.Models.Updates;

namespace android.Services;

public interface IUserService
{
    // Admin
    // Lấy tất cả user theo phân trang
    Task<List<UserResponseModel>> GetAll(int page);
    // Lấy user theo id
    Task<UserResponseModel> GetById(long uid);
    
    // Xóa user
    Task Disable(long uid);
    
    // User
    // sửa thông tin user
    Task Update(long uid, UserUpdateModel model, IFormFile? avatar);

    // Đổi mật khẩu ở bên AuthService
}