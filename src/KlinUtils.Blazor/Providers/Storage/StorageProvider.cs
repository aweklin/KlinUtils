using System.Text.Json;

using Microsoft.JSInterop;

namespace KlinUtils.Blazor.Providers.Storage;

internal sealed class StorageProvider(
  IJSRuntime jsRuntime) : IStorageProvider
{
  private readonly IJSRuntime _jsRuntime =
    jsRuntime ?? throw new ArgumentNullException(nameof(jsRuntime));

  public async ValueTask DeleteAsync(string key)
  {
    await _jsRuntime
      .InvokeVoidAsync("sessionStorage.removeItem", key)
      .ConfigureAwait(false);
  }

  public async ValueTask<T?> GetAsync<T>(string key)
  {
    string? value = await _jsRuntime
      .InvokeAsync<string?>("sessionStorage.getItem", key)
      .ConfigureAwait(false);

    return value is null ? default : JsonSerializer.Deserialize<T>(value);
  }

  public async ValueTask SetAsync<T>(string key, T value)
  {
    ArgumentNullException.ThrowIfNull(value);

    string serializedValue = JsonSerializer.Serialize(value);
    await _jsRuntime
      .InvokeVoidAsync("sessionStorage.setItem", key, serializedValue)
      .ConfigureAwait(false);
  }
}