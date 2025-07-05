namespace KlinUtils.Common.Helpers.Results;

public sealed record ConflictError(
    IEnumerable<ErrorInfo> Errors)
    : Failure(Errors)
{
    public ConflictError(ErrorInfo error)
        : this([error])
    {
    }

    public ConflictError(string error)
        : this([ErrorInfo.Conflict(error)])
    {
    }
}