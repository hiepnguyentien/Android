using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using android.Enumerables;
using android.Services;
using android.Tools;
using android.Models.Updates;
using Microsoft.Net.Http.Headers;
using System.Text.Json;
using android.Exceptions;
using System.Net;
using android.Models.Inserts;
namespace android.Controllers;

[Route("[controller]")]
[ApiController]
public class PlaylistController : ControllerBase
{
    private readonly IPlaylistService _playlistService;
    private readonly IAuthService _authService;
    public PlaylistController(IPlaylistService playlistService, IAuthService authService)
    {
        _playlistService = playlistService;
        _authService = authService;
    }   
    
    [HttpGet("all/{page}")]
    public async Task<IActionResult> GetPublic(int page)
    {
        if (page < 1)
        {
            page = 1;
        }
        var authInformation = await _authService.ValidateToken(Request);
        if (authInformation == null)
        {
            return Ok(await _playlistService.GetAllByGuest(page));
        }
        return authInformation.Role switch
        {
            ERole.ADMIN => Ok(await _playlistService.GetAllByAdmin(page)),
            ERole.USER => Ok(await _playlistService.GetAllByUser(authInformation.UserId)),
            _ => Ok(await _playlistService.GetAllByGuest(page)),
        };
    }

    [HttpGet("{playlistId}")]
    public async Task<IActionResult> GetVia(int playlistId)
    {
        var authInformation = await _authService.ValidateToken(Request);
        if (authInformation != null && authInformation.Role == ERole.USER)
        {
            return Ok(await _playlistService.GetViaIdByUser(playlistId, authInformation.UserId));
        }
        return Ok(await _playlistService.GetViaIdByGuest(playlistId));
    }

    [HttpGet("artwork/{filename}")]
    public IActionResult GetArtwork(string filename)
    {
        return File(FileTool.ReadArtwork(filename), "image/jpeg");
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string keyword)
    {
        var authInformation = await _authService.ValidateToken(Request);
        if (authInformation !=  null && authInformation.Role == ERole.ADMIN)
        {
            return Ok(await _playlistService.SearchByAdmin(keyword));
        }
        return Ok(await _playlistService.SearchByGuest(keyword));
    }

    [HttpGet("{id}/tracks")]
    public async Task<IActionResult> PlaylistByOrder(int id)
    {
        return Ok(await _playlistService.PlaylistByOrder(id));
    }
    
    [HttpGet("{id}/tracks/random")]
    public async Task<IActionResult> PlaylistRandomly(int id)
    {
        return Ok(await _playlistService.PlaylistRandomly(id));
    }

    [HttpPost]
    [AppAuthorize(ERole.USER)]
    public async Task<IActionResult> AddNew(
        [FromForm] string model,
        [FromForm] IFormFile? artwork)
    {
        var playlistInsertModel = JsonSerializer.Deserialize<PlaylistInsertModel>(model)
            ?? throw new AppException(HttpStatusCode.BadRequest, "Dữ liệu không hợp lệ");
        var userId = User.FindFirstValue("userid");
        if (userId != null && long.TryParse(userId, out long uid))
        {
            return Ok(await _playlistService.AddNew(playlistInsertModel, artwork, uid));
        }
        return Forbid();
    }

    [HttpPost("{playlistId}/repost")]
    [AppAuthorize(ERole.USER)]
    public async Task<IActionResult> Repost(int playlistId)
    {
        var userId = User.FindFirstValue("userid");
        if (userId != null && long.TryParse(userId, out long uid))
        {
            await _playlistService.Repost(playlistId, uid);
            return Ok();
        }
        return Forbid();
    }

    [HttpPut("{playlistId}/like")]
    [AppAuthorize(ERole.USER, ERole.ADMIN)]
    public async Task<IActionResult> Like(int playlistId)
    {
        var userId = User.FindFirstValue("userid");
        if (userId != null && long.TryParse(userId, out long uid))
        {
            await _playlistService.Like(playlistId, uid);
            return Ok();
        }
        return Forbid();
    }


    [HttpPut("{playlistId}/information")]
    [AppAuthorize(ERole.USER)]
    public async Task<IActionResult> UpdateInfomation(
        [FromForm] string model,
        IFormFile? artwork, 
        [FromRoute] int playlistId)
    {
        var playlistUpdateModel = JsonSerializer.Deserialize<PlaylistUpdateModel>(model)
            ?? throw new AppException(HttpStatusCode.BadRequest, "Dữ liệu không hợp lệ");
        var userId = User.FindFirstValue("userid");
        if (userId != null && long.TryParse(userId, out long uid))
        {
            await _playlistService.UpdateInfomation(playlistId, playlistUpdateModel, artwork, uid);
            return Ok();
        }
        return Forbid();
    }

    [HttpDelete("{playlistId}")]
    [AppAuthorize(ERole.USER, ERole.ADMIN)]
    public async Task<IActionResult> DeleteByCreator(int playlistId)
    {
        var userId = User.FindFirstValue("userid");
        var role = ERoleTool.ToERole(User.FindFirstValue(ClaimTypes.Role)!);
        if (role == ERole.USER && userId != null && long.TryParse(userId, out long uid))
        {
            await _playlistService.DeleteByCreator(playlistId, uid);
            return Ok();
        }
        await _playlistService.DeleteByAdmin(playlistId);
        return Ok();
    }
}