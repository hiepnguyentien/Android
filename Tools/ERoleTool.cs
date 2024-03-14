using android.Enumerables;

namespace android.Tools;

public class ERoleTool
{
    public static ERole ToERole(string roleString)
    {
        return roleString.ToLower() switch
        {
            "superadmin" => ERole.SUPERADMIN,
            "admin" => ERole.ADMIN,
            "user" => ERole.USER,
            _ => ERole.GUEST,
        };
    }

    public static string ToString(ERole erole)
    {
        return erole.ToString();
    }

    public static ERole GetHighestRole(IList<string> roles)
    {
        if (roles.FirstOrDefault(r => string.Equals(r, "SUPERADMIN", StringComparison.OrdinalIgnoreCase)) != null)
            return ERole.SUPERADMIN;
        else if (roles.FirstOrDefault(r => string.Equals(r, "ADMIN", StringComparison.OrdinalIgnoreCase)) != null)
            return ERole.ADMIN;
        else
            return ERole.USER;
    }
}