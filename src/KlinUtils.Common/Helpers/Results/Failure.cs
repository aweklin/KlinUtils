namespace KlinUtils.Common.Helpers.Results;

public record Failure(IEnumerable<ErrorInfo> Errors)
{
    public static implicit operator Failure(ErrorInfo error)
    {
        return new([error]);
    }

    public static Failure FromErrorInfo(ErrorInfo error)
    {
        return new([error]);
    }

    public static Failure FromMe(Failure failure)
    {
        ArgumentNullException.ThrowIfNull(failure);
        return new Failure(failure.Errors);
    }
}