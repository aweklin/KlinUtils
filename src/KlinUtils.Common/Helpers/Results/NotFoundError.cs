namespace KlinUtils.Common.Helpers.Results;

public sealed record NotFoundError(
    string Message,
    string Code = "NotFound")
    : Failure([new ErrorInfo(Code, Message)]);