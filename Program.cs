using System.Text;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using android.Context;
using android.Entities;
using android.Exceptions;
using android.Services;
using android.Services.Impl;
using android.Services.Impls;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddScoped<ErrorMiddleware>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITrackService, TrackService>();
builder.Services.AddScoped<IPlaylistService, PlaylistService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICommentService, CommentService>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
            policy  =>
                {
                    policy
                        .WithOrigins("http://localhost:44410", "*")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
});

builder.Services.AddDbContext<ApplicationContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

builder.Services.AddAuthentication(FacebookDefaults.AuthenticationScheme)
.AddJwtBearer(jwtOPT => 
{
    jwtOPT.RequireHttpsMetadata = false;
    jwtOPT.SaveToken = true;
    jwtOPT.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidIssuer = config["JwtInfo:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(config["JwtInfo:Key"]!)),
        ValidateIssuer = true, 
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true
    };
});
// .AddFacebook(options =>
// {
//     options.AppId = config["FacebookInfo:AppId"]!;
//     options.AppSecret = config["FacebookInfo:AppSecret"]!;
// })
// .AddGoogle(options => 
// {
//     options.ClientId = config["GoogleInfo:ClientId"]!;
//     options.ClientSecret = config["GoogleInfo:ClientSecret"]!;
// });

builder.Services.AddAuthorization();

builder.Services.AddIdentity<User, Role>(options =>
{
    options.SignIn.RequireConfirmedPhoneNumber = false;
    options.SignIn.RequireConfirmedEmail = false;

    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequiredUniqueChars = 0;

    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);
    options.Lockout.MaxFailedAccessAttempts = 3;
    options.Lockout.AllowedForNewUsers = false;

    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ApplicationContext>()
.AddDefaultTokenProviders();

builder.Services.AddScoped<FakeData>();
var app = builder.Build();

// Fake password cho user, có thể comment sau lần chạy đầu
var scope = app.Services.CreateScope();
await scope.ServiceProvider.GetRequiredService<FakeData>().InitDataAsync();

// if (app.Environment.IsDevelopment())
// {
    // app.UseDeveloperExceptionPage();
// } 
// else
// {
    app.UseMiddleware<ErrorMiddleware>();
// }



// services.AddResponseCaching();


app.UseRouting();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();

// Code react
// app.UseRouting();
// app.UseStaticFiles();
// app.MapControllerRoute(
//     name: "default",
//     pattern: "{controller}/{action=Index}/{id?}");
// app.MapFallbackToFile("index.html");