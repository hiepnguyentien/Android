using System.Net;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using android.Context;
using android.Exceptions;
using android.Models.Responses;
using android.Models.Updates;
using android.Tools;

namespace android.Services.Impl;

public class UserService : IUserService
{
    private readonly int PAGE_SIZE = 10;
    private readonly ApplicationContext _dbContext;
    public UserService(ApplicationContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<UserResponseModel>> GetAll(int page)
    {
        return await _dbContext.Users
            .OrderBy(u => u.Id)
            .Skip((page - 1) * PAGE_SIZE)
            .Take(PAGE_SIZE)
            .Select(u => new UserResponseModel
            {
                Id = u.Id,
                UserName = u.UserName!,
                Email = u.Email!,
                Avatar = UserTool.GetAvatar(u),
                FullName = UserTool.GetAuthorName(u),
                PhoneNumber = u.PhoneNumber!
            })
            .ToListAsync();
    }

    public async Task<UserResponseModel> GetById(long uid)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == uid) 
            ?? throw new AppException(HttpStatusCode.Unauthorized, 
            "Bạn không đủ quyền truy cập");
        return new UserResponseModel() 
        {
            Id = user.Id,
            UserName = user.UserName!,
            Email = user.Email!,
            Avatar = UserTool.GetAvatar(user),
            FullName = UserTool.GetAuthorName(user),
            PhoneNumber = user.PhoneNumber!
        };
    }

    public async Task Update(long uid, UserUpdateModel model, IFormFile? avatar)
    {
        if (await _dbContext.Users.AnyAsync(u => u.UserName == model.UserName))
            throw new AppException(HttpStatusCode.BadRequest, 
                "Tên đăng nhập đã tồn tại");
        if (await _dbContext.Users.AnyAsync(u => u.Email == model.Email))
            throw new AppException(HttpStatusCode.BadRequest, 
                "Email đã tồn tại");
        if (await _dbContext.Users.AnyAsync(u => u.PhoneNumber == model.PhoneNumber))
            throw new AppException(HttpStatusCode.BadRequest, 
                "Số điện thoại đã tồn tại");
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == uid)
            ?? throw new AppException(HttpStatusCode.Forbidden, 
                "Bạn không đủ quyền truy cập");
        user.UserName = model.UserName;
        user.Email = model.Email;
        user.PhoneNumber = model.PhoneNumber;
        if (avatar != null)
        {
            await FileTool.SaveAvatar(avatar);
            user.Avatar = avatar.FileName;
        }
        await _dbContext.SaveChangesAsync();
    }

    public async Task Disable(long uid)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == uid) 
            ?? throw new AppException(HttpStatusCode.NotFound, "Bạn không đủ quyền truy cập");
        if (user.LockoutEnabled)
            return;
        
        // Khoá 1 tháng
        user.LockoutEnabled = true;
        user.LockoutEnd = DateTime.Now.AddDays(30);
        await _dbContext.SaveChangesAsync();
    }
}