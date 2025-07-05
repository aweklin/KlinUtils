namespace KlinUtils.Common.Helpers.Results;

public sealed record UnauthorizedError(
    string Message = "Access denied")
    : Failure([new ErrorInfo("Unauthorized", Message)]);