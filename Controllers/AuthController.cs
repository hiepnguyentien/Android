using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using android.Enumerables;
using android.Models.Securities;
using android.Services;
using android.Tools;

namespace android.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _service;

    public AuthController(IAuthService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult GetRoot()
    {
        return Ok("Truy cập <a href=\"postman.com\">Postman</a> để dùng API");
    }

    [HttpPost("signup")]
    public async Task<IActionResult> Signup([FromBody] SignupModel model)
    {
        return Ok(await _service.SignUpAsync(model));
    }

    [HttpPost("signin")]
    public async Task<IActionResult> Signin([FromBody] SigninModel model)
    {
        return Ok(await _service.SignInAsync(model));
    }

    [HttpGet("signout")]
    [AppAuthorize(ERole.USER)]
    public async Task Signout()
    {
        await _service.SignOutAsync();
    }

    [HttpPost("reset-password")]
    [AppAuthorize(ERole.USER)]
    public async Task<IActionResult> ResetPassword([FromBody] ChangePasswordModel model)
    {
        var userid = User.FindFirstValue("userid");
        if (userid != null && int.TryParse(userid, out int id))
        {
            return Ok(await _service.ChangePasswordAsync(id, model));
        }
        return BadRequest();
    }
}