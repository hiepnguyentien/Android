using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using android.Entities;
using android.Enumerables;
using android.Models.Inserts;
using android.Models.Updates;
using android.Services;
using android.Tools;

namespace android.Controllers;

[ApiController]
[Route("[controller]")]
public class CommentController : ControllerBase
{
    private readonly ICommentService _service;
    public CommentController(ICommentService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllComment()
    {
        return Ok(await _service.GetAll());
    }

    [HttpGet("violation")]
    [AppAuthorize(ERole.ADMIN)]
    public async Task<IActionResult> GetViolationComment()
    {
        return Ok(await _service.GetViolationComment());
    }

    [HttpGet("nonviolation")]
    [AppAuthorize(ERole.ADMIN)]
    public async Task<IActionResult> GetNonViolationComment()
    {
        return Ok(await _service.GetNonViolationComment());
    }

    [HttpGet("search")]
    [AppAuthorize(ERole.ADMIN)]
    public async Task<IActionResult> SearchComment(string query)
    {
        return Ok(await _service.SearchComment(query));
    }

    [HttpGet("searchbyuser")]
    [AppAuthorize(ERole.ADMIN)]
    public async Task<IActionResult> SearchCommentByUser(string query)
    {
        return Ok(await _service.SearchCommentByUser(query));
    }

    [HttpGet("track/{id}")]
    [AppAuthorize(ERole.USER)]
    public async Task<IActionResult> GetCommentByTrackId(int id)
    {
        return Ok(await _service.GetByTrackId(id));
    }

    [HttpPost("track/{id}")]
    [AppAuthorize(ERole.USER)]
    public async Task<IActionResult> Comment([FromBody] CommentInsertModel model, int id)
    {
        var userIdString = User.FindFirstValue("userid");
        if (long.TryParse(userIdString, out var userId))
        {
            await _service.Comment(model, userId, id);
            return Ok();
        }
        return BadRequest("Token không hợp lệ");
    }

    [HttpPut("{id}")]
    [AppAuthorize(ERole.USER)]
    public async Task<IActionResult> UpdateCommentByCreator(int id, CommentUpdateModel model)
    {
        var userIdString = User.FindFirstValue("userid");
        if (long.TryParse(userIdString, out var userId))
        {
            await _service.UpdateCommentByCreator(id, userId, model);
            return Ok();
        }
        return BadRequest("Invalid user id.");
    }

    [HttpPut("report/{id}")]
    [AppAuthorize(ERole.USER)]
    public async Task<IActionResult> ReportComment(int id)
    {
        var userIdString = User.FindFirstValue("userid");
        if (long.TryParse(userIdString, out var userId))
        {
            await _service.ReportComment(id, userId);
            return Ok();
        }
        return BadRequest("Invalid user id.");
    }

    [HttpPut("unreport/{id}")]
    [AppAuthorize(ERole.ADMIN)]
    public async Task<IActionResult> UnReportComment(int id)
    {
        await _service.UnReportComment(id);
        return Ok();
    }

    [HttpDelete("{id}")]
    [AppAuthorize(ERole.USER, ERole.ADMIN)]
    public async Task<IActionResult> DeleteComment(int id)
    {
        var role = ERoleTool.ToERole(User.FindFirstValue(ClaimTypes.Role)!);
        if (role == ERole.ADMIN)
        {
            await _service.DeleteCommentByAdmin(id);
            return Ok();
        }
        else if (role == ERole.USER)
        {
            var userid = long.Parse(User.FindFirstValue("userid")!);
            await _service.DeleteCommentByCreator(id, userid);
            return Ok();
        }
        return Unauthorized();
    }


}