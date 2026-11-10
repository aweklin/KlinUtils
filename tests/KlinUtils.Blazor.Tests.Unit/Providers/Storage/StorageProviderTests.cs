using KlinUtils.Blazor.Providers.Storage;

using Microsoft.JSInterop;

namespace KlinUtils.Blazor.Tests.Unit.Providers.Storage;

public class StorageProviderTests
{
    [Fact]
    public void Constructor_WithNullJsRuntime_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new StorageProvider(null!));
    }

    [Fact]
    public async Task SetAsync_StringValue_StoresJsonSerializedValue()
    {
        FakeJsRuntime jsRuntime = new();
        StorageProvider provider = new(jsRuntime);

        await provider.SetAsync("greeting", "hello");

        JsInvocation invocation = Assert.Single(jsRuntime.Invocations);
        Assert.Equal("sessionStorage.setItem", invocation.Identifier);
        Assert.Equal(["greeting", "\"hello\""], invocation.Arguments);
    }

    [Fact]
    public async Task SetAsync_ObjectValue_StoresJsonSerializedValue()
    {
        FakeJsRuntime jsRuntime = new();
        StorageProvider provider = new(jsRuntime);
        TestValue value = new("Akeem", 42);

        await provider.SetAsync("profile", value);

        JsInvocation invocation = Assert.Single(jsRuntime.Invocations);
        Assert.Equal("sessionStorage.setItem", invocation.Identifier);
        Assert.Equal(["profile", """{"Name":"Akeem","Age":42}"""], invocation.Arguments);
    }

    [Fact]
    public async Task SetAsync_NullValue_ThrowsWithoutCallingJsRuntime()
    {
        FakeJsRuntime jsRuntime = new();
        StorageProvider provider = new(jsRuntime);

        await Assert.ThrowsAsync<ArgumentNullException>(
          async () => await provider.SetAsync<string?>("key", null).ConfigureAwait(false))
          .ConfigureAwait(true);

        Assert.Empty(jsRuntime.Invocations);
    }

    [Fact]
    public async Task GetAsync_StoredValue_DeserializesValue()
    {
        FakeJsRuntime jsRuntime = new((_, _) => """{"Name":"Akeem","Age":42}""");
        StorageProvider provider = new(jsRuntime);

        TestValue? result = await provider.GetAsync<TestValue>("profile");

        Assert.Equal(new TestValue("Akeem", 42), result);
        JsInvocation invocation = Assert.Single(jsRuntime.Invocations);
        Assert.Equal("sessionStorage.getItem", invocation.Identifier);
        Assert.Equal(["profile"], invocation.Arguments);
    }

    [Fact]
    public async Task GetAsync_StoredString_DeserializesString()
    {
        FakeJsRuntime jsRuntime = new((_, _) => "\"hello\"");
        StorageProvider provider = new(jsRuntime);

        string? result = await provider.GetAsync<string>("greeting");

        Assert.Equal("hello", result);
    }

    [Fact]
    public async Task GetAsync_MissingValue_ReturnsDefault()
    {
        FakeJsRuntime jsRuntime = new((_, _) => null);
        StorageProvider provider = new(jsRuntime);

        TestValue? result = await provider.GetAsync<TestValue>("missing");

        Assert.Null(result);
    }

    [Fact]
    public async Task GetAsync_MissingValueForValueType_ReturnsDefault()
    {
        FakeJsRuntime jsRuntime = new((_, _) => null);
        StorageProvider provider = new(jsRuntime);

        int result = await provider.GetAsync<int>("missing");

        Assert.Equal(0, result);
    }

    [Fact]
    public async Task GetAsync_StoredJsonNull_ReturnsNull()
    {
        FakeJsRuntime jsRuntime = new((_, _) => "null");
        StorageProvider provider = new(jsRuntime);

        string? result = await provider.GetAsync<string>("value");

        Assert.Null(result);
    }

    [Fact]
    public async Task GetAsync_InvalidJson_ThrowsJsonException()
    {
        FakeJsRuntime jsRuntime = new((_, _) => "{invalid");
        StorageProvider provider = new(jsRuntime);

        await Assert.ThrowsAsync<System.Text.Json.JsonException>(
          async () => await provider.GetAsync<TestValue>("profile").ConfigureAwait(false))
          .ConfigureAwait(true);
    }

    [Fact]
    public async Task DeleteAsync_RemovesItemWithKey()
    {
        FakeJsRuntime jsRuntime = new();
        StorageProvider provider = new(jsRuntime);

        await provider.DeleteAsync("profile");

        JsInvocation invocation = Assert.Single(jsRuntime.Invocations);
        Assert.Equal("sessionStorage.removeItem", invocation.Identifier);
        Assert.Equal(["profile"], invocation.Arguments);
    }

    [Theory]
    [InlineData("sessionStorage.setItem")]
    [InlineData("sessionStorage.getItem")]
    [InlineData("sessionStorage.removeItem")]
    public async Task StorageOperation_WhenJsInteropFails_PropagatesException(string operation)
    {
        InvalidOperationException exception = new("Interop failed.");
        FakeJsRuntime jsRuntime = new((identifier, _) =>
        {
            if (identifier == operation)
            {
                throw exception;
            }

            return null;
        });
        StorageProvider provider = new(jsRuntime);

        InvalidOperationException thrown = await Assert.ThrowsAsync<InvalidOperationException>(
          async () =>
          {
              switch (operation)
              {
                  case "sessionStorage.setItem":
                      await provider.SetAsync("key", "value").ConfigureAwait(false);
                      break;
                  case "sessionStorage.getItem":
                      await provider.GetAsync<string>("key").ConfigureAwait(false);
                      break;
                  default:
                      await provider.DeleteAsync("key").ConfigureAwait(false);
                      break;
              }
          }).ConfigureAwait(true);

        Assert.Same(exception, thrown);
    }

    private sealed record TestValue(string Name, int Age);

    private sealed record JsInvocation(string Identifier, object?[]? Arguments);

    private sealed class FakeJsRuntime(Func<string, object?[]?, object?>? handler = null) : IJSRuntime
    {
        public List<JsInvocation> Invocations { get; } = [];

        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object?[]? args)
        {
            return InvokeAsync<TValue>(identifier, CancellationToken.None, args);
        }

        public ValueTask<TValue> InvokeAsync<TValue>(
          string identifier,
          CancellationToken cancellationToken,
          object?[]? args)
        {
            Invocations.Add(new JsInvocation(identifier, args));
            object? result = handler?.Invoke(identifier, args);
            return ValueTask.FromResult(result is null ? default! : (TValue)result);
        }
    }
}