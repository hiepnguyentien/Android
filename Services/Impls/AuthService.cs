using android.Models.Securities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using android.Entities;
using android.Exceptions;
using android.Enumerables;
using android.Tools;

namespace android.Services.Impls;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IConfiguration _config;
    public AuthService(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration config)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _config = config;
    }

    public async Task<ResponseModel> SignInAsync(SigninModel model)
    {
        var user = await _userManager.FindByNameAsync(model.UserName) 
            ?? throw new AppException(HttpStatusCode.NotFound, 
                "Tài khoản không tồn tại");
                
        var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, true);
        if (!result.Succeeded)
        {
            if (result.IsLockedOut)
            {
                return new ResponseModel()
                {
                    IsSucceed = false,
                    Data = "Tài khoản đã bị khóa"
                };
            }
            return new ResponseModel()
            {
                IsSucceed = false,
                Data = "Tài khoản hoặc mật khẩu không chính xác"
            };
        }
        var highestRole = ERoleTool.GetHighestRole(await _userManager.GetRolesAsync(user));
        return new ResponseModel()
        {
            IsSucceed = true,
            Data = GenerateJwtToken(user, highestRole)
        };
    }

    public async Task<ResponseModel> SignUpAsync(SignupModel model)
    {
        var user = new User()
        {
            UserName = model.UserName,
            Email = model.Email,
            PhoneNumber = model.PhoneNumber,
        };
        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
        {
            foreach (var err in result.Errors)
            {
                switch (err.Code)
                {
                    case "DuplicateUserName":
                        throw new AppException(HttpStatusCode.Conflict, 
                                $"Tên đăng nhập '{model.UserName}' đã tồn tại");
                    case "DuplicateEmail":
                        throw new AppException(HttpStatusCode.Conflict, 
                                $"Email '{model.Email}' đã tồn tại");
                    default:
                        Console.WriteLine($"Error: {err.Description}");
                        throw new AppException(HttpStatusCode.BadRequest, 
                            "Đăng kí không thành công");
                }
            }
        }
        await _userManager.AddToRoleAsync(user, ERole.USER.ToString());
        return new ResponseModel()
        {
            IsSucceed = true,
            Data = GenerateJwtToken(user, ERole.USER)
        };
    }

    private string GenerateJwtToken(User user, ERole erole)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var claims = new ClaimsIdentity(new[]
        {
            new Claim("userid", user.Id.ToString(), ClaimValueTypes.Integer64),
            new Claim(ClaimTypes.Role, ERoleTool.ToString(erole)),
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim(ClaimTypes.Email, user.Email!),
        });
        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["JwtInfo:Key"]!)),
                SecurityAlgorithms.HmacSha512Signature);
        var expireTime = DateTime.UtcNow.AddDays(
            _config.GetSection("JwtInfo:ExpireDays").Get<int>());
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = claims,
            Expires = expireTime,
            SigningCredentials = credentials,
            Issuer = _config["JwtInfo:Issuer"]!,
        };
        var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
    
    public async Task<AuthInformation?> ValidateToken(HttpRequest request) 
    {
        var header = request.Headers["Authorization"];
        if (string.IsNullOrWhiteSpace(header))
        {
            return null;
        }
        var token = header.First()!.Replace("Bearer ", "");
        if (string.IsNullOrWhiteSpace(token))
        {
            return null;
        }
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenResult = await tokenHandler.ValidateTokenAsync(token, new TokenValidationParameters()
        {
            ValidIssuer = _config["JwtInfo:Issuer"]!,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["JwtInfo:Key"]!)),
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true
        });
        if (!tokenResult.IsValid)
        {
            return null;
        }
        return new AuthInformation()
        {
            UserId = long.Parse(tokenResult.Claims.First(claim => claim.Key == "userid").Value.ToString()!),
            UserName = tokenResult.Claims.First(claim => claim.Key == ClaimTypes.Name).Value.ToString()!,
            Email = tokenResult.Claims.First(claim => claim.Key == ClaimTypes.Email).Value.ToString()!,
            Role = ERoleTool.ToERole(tokenResult.Claims.First(claim => claim.Key == ClaimTypes.Role).Value.ToString()!)
        };
    }

    public async Task SignOutAsync()
    {
        await _signInManager.SignOutAsync();
    }

    public async Task<ResponseModel> ChangePasswordAsync(long userId, ChangePasswordModel model)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString()) 
            ?? throw new AppException(HttpStatusCode.NotFound, 
                "Tài khoản không tồn tại");
        var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
        if (!result.Succeeded)
        {
            foreach (var err in result.Errors)
            {
                switch (err.Code)
                {
                    case "PasswordMismatch":
                        throw new AppException(HttpStatusCode.BadRequest, 
                                "Mật khẩu cũ không chính xác");
                    default:
                        Console.WriteLine($"Error: {err.Description}");
                        throw new AppException(HttpStatusCode.BadRequest, 
                            "Đổi mật khẩu không thành công");
                }
            }
        }
        return new ResponseModel()
        {
            IsSucceed = true,
            Data = "Đổi mật khẩu thành công"
        };
    }
}