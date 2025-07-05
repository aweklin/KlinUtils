namespace KlinUtils.Common.Helpers.Results;

public readonly struct Result<TSuccess, TError> : IEquatable<Result<TSuccess, TError>>
    where TError : Failure
{
    private readonly TSuccess? _success;
    private readonly TError? _failure;

    private Result(TSuccess success)
    {
        _success = success;
        _failure = default;
        HasError = false;
    }

    private Result(TError error)
    {
        _success = default;
        _failure = error;
        HasError = true;
    }

    public bool HasError { get; }

    public TSuccess SuccessValue => GetSuccess<TSuccess>();

    public TError FailureValue => GetFailure<TError>();

    public static implicit operator Result<TSuccess, TError>(TSuccess success)
    {
        return new(success);
    }

    public static implicit operator Result<TSuccess, TError>(TError error)
    {
        return new(error);
    }

    public static bool operator ==(Result<TSuccess, TError> left, Result<TSuccess, TError> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Result<TSuccess, TError> left, Result<TSuccess, TError> right)
    {
        return !(left == right);
    }

    public static Result<TSuccess, TError> Success(TSuccess success)
    {
        ArgumentNullException.ThrowIfNull(success);
        return new Result<TSuccess, TError>(success);
    }

    public static Result<TSuccess, TError> Failure(TError failure)
    {
        ArgumentNullException.ThrowIfNull(failure);
        return new Result<TSuccess, TError>(failure);
    }

    public static Result<TSuccess, TError> FromTSuccess(TSuccess success)
    {
        return new(success);
    }

    public static Result<TSuccess, TError> FromTError(TError error)
    {
        return new(error);
    }

    public TResult Match<TResult>(Func<TSuccess, TResult> success, Func<TError, TResult> failure)
    {
        ArgumentNullException.ThrowIfNull(success);
        ArgumentNullException.ThrowIfNull(failure);

        return _success is not null ? success(_success) : failure(_failure!);
    }

    public async ValueTask<TResult> Match<TResult>(
        Func<TSuccess, ValueTask<TResult>> success,
        Func<TError, ValueTask<TResult>> failure)
    {
        ArgumentNullException.ThrowIfNull(success);
        ArgumentNullException.ThrowIfNull(failure);

        return _success is not null
            ? await success(_success).ConfigureAwait(false)
            : await failure(_failure!).ConfigureAwait(false);
    }

    public override bool Equals(object? obj)
    {
        return obj is Result<TSuccess, TError> result && Equals(result);
    }

    public bool Equals(Result<TSuccess, TError> other)
    {
        return EqualityComparer<TSuccess?>.Default.Equals(_success, other._success) &&
               EqualityComparer<TError?>.Default.Equals(_failure, other._failure) &&
               HasError == other.HasError;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_success, _failure, HasError);
    }

    private T GetSuccess<T>()
        where T : TSuccess
    {
        return HasError ? throw new InvalidOperationException("Operation did not succeed.") : (T)_success!;
    }

    private T GetFailure<T>()
        where T : TError
    {
        return !HasError ? throw new InvalidOperationException("Operation succeed.") : (T)_failure!;
    }
}