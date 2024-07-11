using System.Text;

using KlinUtils.Guards;

namespace KlinUtils.DiscriminatedUnion;

public sealed record Error
{
    public string Code { get; }
    public string? Description { get; }

    public Error(string code, string? description = null)
    {
        Code = code;
        Description = description ?? string.Empty;
    }

    public Error(string code, Exception exception)
    {
        Code = code;
        Description = new StringBuilder()
            .AppendLine(exception.Message)
            .AppendLine(exception.InnerException?.Message)
            .AppendLine(exception.StackTrace)
            .ToString();
    }

    public static readonly Error None = new(string.Empty);
    public static Error WithMessage(string message)
    {
        Ensure.NotNullOrEmpty(message);

        return new("Error.WithMessage", message);
    }

    public static readonly Error NullValue = new("Error.Null", "Null value was provided.");
    public static Error Required(string param)
    {
        Ensure.NotNullOrEmpty(param);

        return new("Error.Required", $"{param} is required.");
    }

    public static implicit operator Result(Error error) => Result.Failure(error);

    public Result ToResult() => Result.Failure(this);
}