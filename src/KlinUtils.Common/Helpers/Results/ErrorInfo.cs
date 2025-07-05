namespace KlinUtils.Common.Helpers.Results;

public record struct ErrorInfo(string Code, string Message)
{
    public static ErrorInfo BadRequest(string message = "Invalid request")
    {
        return new("BadRequest", message);
    }

    public static ErrorInfo NotFound(string message = "Not found")
    {
        return new("NotFound", message);
    }

    public static ErrorInfo Conflict(string message = "Conflict")
    {
        return new("Conflict", message);
    }

    public static ErrorInfo Unauthorized(string message = "Access denied")
    {
        return new("Unauthorized", message);
    }

    public static ErrorInfo Forbidden(string message = "Forbidden")
    {
        return new("Forbidden", message);
    }

    public static ErrorInfo InternalServerError(string message = "Internal server error")
    {
        return new("InternalServerError", message);
    }
}