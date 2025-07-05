namespace KlinUtils.Common.Helpers.Results;

public sealed record ValidationError(
    IEnumerable<ErrorInfo> Errors)
    : Failure(Errors)
{
    public ValidationError(ErrorInfo error)
        : this([error])
    {
    }

    public ValidationError(string error)
        : this([ErrorInfo.BadRequest(error)])
    {
    }
}