namespace KlinUtils.DiscriminatedUnion;

// Inspired by Milan Jovanović in his video: https://www.youtube.com/watch?v=WCCkEe_Hy2Y
public class Result<TValue> : Result
{
    private readonly TValue? _value;
    protected internal Result(Error error, TValue? value)
        : base(error)
    {
        _value = value;
    }

    public TValue Value => IsSuccess ?
        _value! :
        throw new InvalidOperationException(
            "The value of a failure result can't be accessed");

    public TNextValue Match<TNextValue>(
        Func<TValue, TNextValue> onSuccess,
        Func<List<Error>, TNextValue> onError)
    {
        return HasError ? onError(Errors) : onSuccess(Value);
    }

    public async ValueTask<TNextValue> MatchAsync<TNextValue>(
        Func<TValue, ValueTask<TNextValue>> onSuccess,
        Func<List<Error>, ValueTask<TNextValue>> onError)
    {
        return HasError ?
            await onError(Errors).ConfigureAwait(false) :
            await onSuccess(Value).ConfigureAwait(false);
    }

    public static implicit operator Result<TValue>(TValue? value) =>
        value is not null ? Success(value) : Failure<TValue>(Error.NullValue);
}