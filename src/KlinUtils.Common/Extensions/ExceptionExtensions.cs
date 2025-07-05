namespace KlinUtils.Common.Extensions;

public static class ExceptionExtensions
{
    /// <summary>
    /// Returns exception message. When a see inner exception message is set as the message, it recursively checks the inner exceptions till it gets the real error message.
    /// </summary>
    /// <param name="exception">System.Exception.</param>
    /// <returns>System.String.</returns>
    public static string GetDetails(this Exception exception)
    {
        ArgumentNullException.ThrowIfNull(exception);

        while (exception.Message.Contains(
            "see inner exception",
            StringComparison.CurrentCultureIgnoreCase)
            && exception.InnerException is not null)
        {
            return GetDetails(exception.InnerException!);
        }

        return exception.Message;
    }
}