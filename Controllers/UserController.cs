using System.Net;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using android.Enumerables;
using android.Exceptions;
using android.Models.Updates;
using android.Services;
using android.Tools;

namespace android.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    [AppAuthorize(ERole.ADMIN)]
    public async Task<IActionResult> GetAll([FromQuery] int page)
    {
        if (page < 1)
        {
            page = 1;
        }
        return Ok(await _userService.GetAll(page));
    }

    [HttpGet("{id}")]
    [AppAuthorize(ERole.USER)]
    public async Task<IActionResult> GetById(int id)
    {
        return Ok(await _userService.GetById(id));
    }

    [HttpDelete]
    [AppAuthorize(ERole.USER)]
    public async Task<IActionResult> Disable()
    {
        var userid = User.FindFirstValue("userid");
        if (userid != null && long.TryParse(userid, out long uid))
        {
            await _userService.Disable(uid);
            return Ok();
        }
        return Forbid();
    }

    [HttpDelete("{id}")]
    [AppAuthorize(ERole.ADMIN)]
    public async Task<IActionResult> DisableByAdmin(int id)
    {
        var userid = User.FindFirstValue("userid");
        if (userid != null && long.TryParse(userid, out long uid))
        {
            if (uid == id)
                return BadRequest("Không thể tự khóa chính mình");
            await _userService.Disable(uid);
            return Ok("Xoá thành công");
        }
        return Forbid();
    }


    [HttpPut]
    [AppAuthorize(ERole.USER)]
    public async Task UpdateInformation(
        [FromForm] string model,
        [FromForm] IFormFile? avatar)
    {
        var updateModel = JsonSerializer.Deserialize<UserUpdateModel>(model);
        var userid = User.FindFirstValue("userid");
        if (updateModel == null)
        {
            throw new AppException(HttpStatusCode.BadRequest, 
                "Dữ liệu không hợp lệ");
        }
        if (userid != null && long.TryParse(userid, out long uid))
        {
            await _userService.Update(uid, updateModel, avatar);
            return;
        }
        throw new AppException(HttpStatusCode.Forbidden, 
            "Ban không đủ quyền truy cập");
    }
}