using System.Net;

namespace android.Exceptions;

public class AppException : Exception
{
    public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.InternalServerError;
    public string Reason { get; set; } = null!;

    public AppException(HttpStatusCode statusCode, string reason)
    {
        StatusCode = statusCode;
        Reason = reason;
    }
}