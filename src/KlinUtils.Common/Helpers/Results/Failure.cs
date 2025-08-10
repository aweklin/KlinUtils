using System.Text;

namespace KlinUtils.Common.Helpers.Results;

public record Failure(IEnumerable<ErrorInfo> Errors)
{
    public ErrorInfo? FirstError => Errors.FirstOrDefault();

    public string FirstErrorAsString => FirstError?.Message ?? string.Empty;

    public string AllErrorsAsString
    {
        get
        {
            if (string.IsNullOrWhiteSpace(FirstErrorAsString))
            {
                return string.Empty;
            }

            StringBuilder errors = new(Errors.Count());
            foreach (ErrorInfo errorInfo in Errors)
            {
                errors.AppendLine(errorInfo.Message);
            }

            return errors.ToString();
        }
    }

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