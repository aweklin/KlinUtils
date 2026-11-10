using KlinUtils.Common.Monads.Options;

namespace KlinUtils.Common.Tests.Unit.Monads.Options;

public class ValueOptionTests
{
  [Fact]
  public void Some_WithValue_HasValueAndReturnsValue()
  {
    // Given
    ValueOption<int> option = ValueOption<int>.Some(123);

    // When
    bool hasValue = option.HasValue;
    int value = option.Value;

    // Then
    Assert.True(hasValue);
    Assert.Equal(123, value);
  }

  [Fact]
  public void None_HasNoValueAndValueThrowsInvalidOperationException()
  {
    // Given
    ValueOption<string> option = ValueOption<string>.None();

    // When + Then
    Assert.False(option.HasValue);
    Assert.Throws<InvalidOperationException>(() => _ = option.Value);
  }

  [Fact]
  public void ImplicitConversion_FromValue_CreatesSomeOption()
  {
    // Given
    ValueOption<string> option = "hello";

    // When
    bool hasValue = option.HasValue;
    string value = option.Value;

    // Then
    Assert.True(hasValue);
    Assert.Equal("hello", value);
  }

  [Fact]
  public void Some_NullValue_ThrowsArgumentNullException()
  {
    // When + Then
    Assert.Throws<ArgumentNullException>(() => ValueOption<string>.Some(null!));
  }

  [Fact]
  public void Match_WithSome_ReturnsSomeResult()
  {
    // Given
    ValueOption<int> option = ValueOption<int>.Some(5);

    // When
    string result = option.Match(
        some: value => $"Value:{value}",
        none: () => "None");

    // Then
    Assert.Equal("Value:5", result);
  }

  [Fact]
  public void Match_WithNone_ReturnsNoneResult()
  {
    // Given
    ValueOption<string> option = ValueOption<string>.None();

    // When
    string result = option.Match(
        some: value => $"Value:{value}",
        none: () => "None");

    // Then
    Assert.Equal("None", result);
  }

  [Fact]
  public void Match_NullSome_ThrowsArgumentNullException()
  {
    // Given
    ValueOption<int> option = ValueOption<int>.None();

    // When + Then
    Assert.Throws<ArgumentNullException>(() => option.Match<int>(null!, () => 0));
  }

  [Fact]
  public void Match_NullNone_ThrowsArgumentNullException()
  {
    // Given
    ValueOption<int> option = ValueOption<int>.None();

    // When + Then
    Assert.Throws<ArgumentNullException>(() => option.Match(value => value, null!));
  }

  [Fact]
  public void ValueOr_WithSome_ReturnsValue()
  {
    // Given
    ValueOption<string> option = ValueOption<string>.Some("present");

    // When
    string result = option.ValueOr(() => "fallback");

    // Then
    Assert.Equal("present", result);
  }

  [Fact]
  public void ValueOr_WithNone_InvokesValueProvider()
  {
    // Given
    ValueOption<string> option = ValueOption<string>.None();

    // When
    string result = option.ValueOr(() => "fallback");

    // Then
    Assert.Equal("fallback", result);
  }

  [Fact]
  public void ValueOr_NullProvider_ThrowsArgumentNullException()
  {
    // Given
    ValueOption<string> option = ValueOption<string>.None();

    // When + Then
    Assert.Throws<ArgumentNullException>(() => option.ValueOr(null!));
  }

  [Fact]
  public void Equals_SameSomeValues_ReturnsTrue()
  {
    // Given
    ValueOption<int> left = ValueOption<int>.Some(10);
    ValueOption<int> right = ValueOption<int>.Some(10);

    // When
    bool result = ValueOption<int>.Equals(left, right);

    // Then
    Assert.True(result);
    Assert.True(left.Equals(right));
    Assert.True(left == right);
    Assert.False(left != right);
  }

  [Fact]
  public void Equals_DifferentSomeValues_ReturnsFalse()
  {
    // Given
    ValueOption<int> left = ValueOption<int>.Some(10);
    ValueOption<int> right = ValueOption<int>.Some(20);

    // When
    bool result = ValueOption<int>.Equals(left, right);

    // Then
    Assert.False(result);
    Assert.False(left.Equals(right));
    Assert.False(left == right);
    Assert.True(left != right);
  }

  [Fact]
  public void Equals_NoneAndNone_ReturnsTrue()
  {
    // Given
    ValueOption<string> left = ValueOption<string>.None();
    ValueOption<string> right = ValueOption<string>.None();

    // When
    bool result = ValueOption<string>.Equals(left, right);

    // Then
    Assert.True(result);
    Assert.True(left.Equals(right));
    Assert.True(left == right);
  }

  [Fact]
  public void Equals_NoneAndSome_ReturnsFalse()
  {
    // Given
    ValueOption<string> left = ValueOption<string>.None();
    ValueOption<string> right = ValueOption<string>.Some("value");

    // When
    bool result = ValueOption<string>.Equals(left, right);

    // Then
    Assert.False(result);
    Assert.False(left.Equals(right));
    Assert.False(left == right);
  }

  [Fact]
  public void GetHashCode_EqualOptions_ReturnSameHashCode()
  {
    // Given
    ValueOption<int> left = ValueOption<int>.Some(42);
    ValueOption<int> right = ValueOption<int>.Some(42);

    // When
    int leftHash = left.GetHashCode();
    int rightHash = right.GetHashCode();

    // Then
    Assert.Equal(leftHash, rightHash);
  }

  [Fact]
  public void GetHashCode_None_ReturnsZero()
  {
    // Given
    ValueOption<string> option = ValueOption<string>.None();

    // When
    int hash = option.GetHashCode();

    // Then
    Assert.Equal(0, hash);
  }

  [Fact]
  public void ToValueOption_ReturnsSameOption()
  {
    // Given
    ValueOption<string> option = ValueOption<string>.Some("same");

    // When
    ValueOption<string> result = option.ToValueOption();

    // Then
    Assert.Equal(option, result);
  }
}