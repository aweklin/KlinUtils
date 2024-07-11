namespace KlinUtils.Extensions;

public static class ExceptionExtensions
{
    public static string GetDetails(this Exception exception)
    {
        while (exception.Message.Contains(
            "see inner exception",
            StringComparison.CurrentCultureIgnoreCase))
        {
            return GetDetails(exception.InnerException!);
        }

        return exception.Message;
    }
}