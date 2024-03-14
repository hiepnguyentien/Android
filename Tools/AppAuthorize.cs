using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using android.Enumerables;

namespace android.Tools;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AppAuthorizeAttribute : AuthorizeAttribute
{
    public AppAuthorizeAttribute(params ERole[] eroles)
    {
        Roles = string.Join(", ", eroles.Select(r => ERoleTool.ToString(r)));
        AuthenticationSchemes = @$"{JwtBearerDefaults.AuthenticationScheme}";
    }
}