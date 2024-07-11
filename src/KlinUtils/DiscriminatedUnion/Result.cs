using System.Text;

using KlinUtils.Guards;

namespace KlinUtils.DiscriminatedUnion;

// Inspired by Milan Jovanović in his video: https://www.youtube.com/watch?v=WCCkEe_Hy2Y
public class Result
{
    protected internal Result(Error error)
    {
        if (!Errors.Contains(error) && error != Error.None)
        {
            Ensure.NotNullOrEmpty(error.Code);
            Errors.Add(error);
        }
    }

    public bool IsSuccess => Errors.Count == 0;
    public bool HasError => !IsSuccess;

    public List<Error> Errors { get; private set; } = [];

    public Error FirstError() => Errors switch
    {
    [] => Error.None,
        _ => Errors.First(),
    };

    public Error LastError() => Errors switch
    {
    [] => Error.None,
        _ => Errors.Last(),
    };

    public string ErrorsAsString()
    {
        StringBuilder errorBuilder = new();

        foreach (Error error in Errors)
        {
            errorBuilder.AppendLine(error.Description);
        }

        return errorBuilder.ToString();
    }

    public static Result Success() => new(Error.None);
    public static Result Failure(Error error) => new(error);
    public static Result Failures(List<Error> errors)
    {
        if (errors.Count == 0)
        {
            throw new InvalidOperationException("At least, one error must be supplied.");
        }

        if (errors.Contains(Error.None))
        {
            throw new InvalidOperationException("You cannot have a Error.None in your failures.");
        }

        Result result = new(Error.None);
        result.Errors.Clear();
        HashSet<Error> uniqueErrors = new(errors);
        result.Errors = [.. uniqueErrors];

        return result;
    }

    public static Result Failures<T>(List<Error> errors)
    {
        if (errors.Count == 0)
        {
            throw new InvalidOperationException("At least, one error must be supplied.");
        }

        if (errors.Contains(Error.None))
        {
            throw new InvalidOperationException("You cannot have a Error.None in your failures.");
        }

        Result<T> result = new(Error.None, default);
        result.Errors.Clear();
        HashSet<Error> uniqueErrors = new(errors);
        result.Errors = [.. uniqueErrors];

        return result;
    }

    public static Result Failure(string error) => new(new Error("Failure", error));
    public static readonly Result Unknown =
        new(new Error("An unknown error occurred but has been escalated for a fix."));
    public static Result<TValue> Success<TValue>(TValue value) => new(Error.None, value);
    public static Result<TValue> Failure<TValue>(Error error) => new(error, default);
}