namespace KlinUtils.Common.Monads.Options;

public readonly struct ValueOption<T> : IEquatable<ValueOption<T>>
{
  private readonly T? _value;

  private ValueOption(T? value)
  {
    _value = value;
    HasValue = true;
  }

  public bool HasValue { get; }

  public T Value => _value ?? throw new InvalidOperationException("No value present.");

  public static implicit operator ValueOption<T>(T value)
  {
    return new(value);
  }

  public static bool operator ==(ValueOption<T> left, ValueOption<T> right)
  {
    return left.Equals(right);
  }

  public static bool operator !=(ValueOption<T> left, ValueOption<T> right)
  {
    return !(left == right);
  }

  public static ValueOption<T> Some(T value)
  {
    ArgumentNullException.ThrowIfNull(value);
    return new ValueOption<T>(value);
  }

  public static bool Equals(ValueOption<T> left, ValueOption<T> right)
  {
    if (left.HasValue != right.HasValue)
    {
      return false;
    }

    return !left.HasValue || EqualityComparer<T>.Default.Equals(left._value!, right._value!);
  }

  public static ValueOption<T> None()
  {
    return default;
  }

  public ValueOption<T> ToValueOption()
  {
    return this;
  }

  public TOut Match<TOut>(Func<T, TOut> some, Func<TOut> none)
  {
    ArgumentNullException.ThrowIfNull(some);
    ArgumentNullException.ThrowIfNull(none);

    return HasValue ? some(Value) : none();
  }

  public T ValueOr(Func<T> valueProvider)
  {
    ArgumentNullException.ThrowIfNull(valueProvider);
    return HasValue ? Value : valueProvider();
  }

  public override bool Equals(object? obj)
  {
    return obj is ValueOption<T> option && Equals(option);
  }

  public bool Equals(ValueOption<T> other)
  {
    return Equals(this, other);
  }

  public override int GetHashCode()
  {
    return HasValue
      ? EqualityComparer<T>.Default.GetHashCode(_value!)
      : 0;
  }
}