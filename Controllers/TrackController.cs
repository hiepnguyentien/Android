using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using android.Enumerables;
using android.Models.Inserts;
using android.Models.Updates;
using android.Services;
using android.Tools;
namespace android.Controllers;

[ApiController]
[Route("[controller]")]
public class TrackController : ControllerBase
{
    private readonly ITrackService _trackService;
    private readonly IAuthService _authService;
    public TrackController(ITrackService trackService, IAuthService authService)
    {
        _trackService = trackService;
        _authService = authService;
    }

    [HttpGet("all/{page:int}")]
    public async Task<IActionResult> GetAllTrack(int page)
    {
        if (page < 1)
        {
            page = 1;
        }
        var authInformation = await _authService.ValidateToken(Request);
        if (authInformation != null)
        {
            return authInformation.Role switch
            {
                ERole.ADMIN => Ok(await _trackService.GetAllByAdmin(page)),
                ERole.USER => Ok(await _trackService.GetAllByGuestByUser(authInformation.UserId, page)),
                _ => Ok(await _trackService.GetAllByGuest(page)),
            };
        }
        return Ok(await _trackService.GetAllByGuest(page));
    }

    [HttpGet("library")]
    [AppAuthorize(ERole.USER)]
    public async Task<IActionResult> GetAllLibrary()
    {
        var userId = User.FindFirstValue("userid");
        if (userId != null && int.TryParse(userId, out int uid))
        {
            return Ok(await _trackService.GetAllUploadedByUser(uid));
        }
        return Unauthorized();
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var authInformation = await _authService.ValidateToken(Request);
        if (authInformation != null)
        {
            return Ok(await _trackService.GetById(id, authInformation.Role, authInformation.UserId));
        }
        return Ok(await _trackService.GetById(id, ERole.GUEST, 0));
    }

    [HttpGet("media/{filename}")]
    public async Task<IActionResult> PlayTrack([FromRoute] string filename)
    {
        var authInformation = await _authService.ValidateToken(Request);
        if (authInformation != null)
        {
            return File(await _trackService.PlayTrack(filename, authInformation.UserId),
                "audio/mpeg",
                enableRangeProcessing: true);
        }
        return File(await _trackService.PlayTrack(filename, 0),
            "audio/mpeg",
            enableRangeProcessing: true);
    }

    [HttpGet("artwork/{filename}")]
    public IActionResult GetArtworkTrack([FromRoute] string filename)
    {
        return File(FileTool.ReadArtwork(filename), "image/jpeg");
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string keyword)
    {
        var authInformation = await _authService.ValidateToken(Request);
        if (authInformation == null)
        {
            return Ok(await _trackService.SearchByGuest(keyword));
        }
        return authInformation.Role switch
        {
            ERole.ADMIN => Ok(await _trackService.SearchByAdmin(keyword)),
            ERole.USER => Ok(await _trackService.SearchByUser(keyword, authInformation.UserId)),
            _ => Ok(await _trackService.SearchByGuest(keyword)),
        };
    }

    [HttpPost]
    [AppAuthorize(ERole.USER)]
    public async Task<IActionResult> Addnew(
        [FromForm] string model,
        IFormFile? fileArtwork,
        IFormFile fileTrack)
    {
        var uid = User.FindFirstValue("userid");
        if (uid != null && long.TryParse(uid, out long userId) && model != null)
        {
            var updateModel = JsonSerializer.Deserialize<TrackInsertModel>(model);
            if (updateModel == null)
            {
                return BadRequest("Model sai");
            }
            await _trackService.AddNew(updateModel, userId, fileTrack, fileArtwork);
            return Ok();
        }
        return BadRequest();
    }

    [HttpPut("{trackId}")]
    [AppAuthorize(ERole.USER)]
    public async Task<IActionResult> Update(
        int trackId,
        [FromForm] string model,
        [FromForm] IFormFile? fileArtwork)
    {
        var trackUpdateModel = JsonSerializer.Deserialize<TrackUpdateModel>(model);
        if (trackUpdateModel == null)
        {
            return BadRequest("Model sai");
        }
        var userId = User.FindFirstValue("userid");
        if (userId != null && long.TryParse(userId, out long uid))
        {
            await _trackService.UpdateInfomation(trackUpdateModel, fileArtwork, trackId, uid);
            return Ok();
        }
        return BadRequest();
    }

    [HttpPut("like/{id}")]
    [AppAuthorize(ERole.USER)]
    public async Task<IActionResult> Like(int id)
    {
        var userId = User.FindFirstValue("userid");
        if (userId != null && int.TryParse(userId, out int uid))
        {
            await _trackService.LikeTrack(uid, id);
            return Ok();
        }
        return BadRequest();
    }

    [HttpDelete("{id}")]
    [AppAuthorize(ERole.USER)]
    public async Task<IActionResult> Delete(int id)
    {
        await _trackService.Remove(id);
        return Ok();
    }
}