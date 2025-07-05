using System.Diagnostics.CodeAnalysis;

namespace KlinUtils.Common.Helpers.Results;

public sealed record UnexpectedError(
    IEnumerable<ErrorInfo> Errors)
    : Failure(Errors)
{
    public UnexpectedError(ErrorInfo error)
        : this([error])
    {
    }

    public UnexpectedError([NotNull] Exception exception)
        : this([new ErrorInfo("UnexpectedError", exception.Message)])
    {
    }

    public UnexpectedError()
        : this([new ErrorInfo("UnexpectedError", "An unexpected error occurred")])
    {
    }
}