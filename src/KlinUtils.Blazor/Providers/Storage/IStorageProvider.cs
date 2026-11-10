namespace KlinUtils.Blazor.Providers.Storage;

/// <summary>
/// Provides asynchronous access to browser session storage.
/// Values are serialized as JSON and stored for the lifetime of the browser tab's session.
/// </summary>
public interface IStorageProvider
{
  /// <summary>
  /// Serializes and stores a value under the specified key.
  /// </summary>
  /// <typeparam name="T">The type of the value to store.</typeparam>
  /// <param name="key">The key under which to store the value.</param>
  /// <param name="value">The value to serialize and store.</param>
  /// <returns>A task that represents the asynchronous storage operation.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
  ValueTask SetAsync<T>(string key, T value);

  /// <summary>
  /// Retrieves and deserializes a value stored under the specified key.
  /// </summary>
  /// <typeparam name="T">The type to deserialize the stored value into.</typeparam>
  /// <param name="key">The key of the value to retrieve.</param>
  /// <returns>
  /// A task containing the deserialized value, or the default value of <typeparamref name="T"/>
  /// if the key does not exist.
  /// </returns>
  /// <exception cref="System.Text.Json.JsonException">The stored value is not valid JSON for <typeparamref name="T"/>.</exception>
  ValueTask<T?> GetAsync<T>(string key);

  /// <summary>
  /// Removes the value stored under the specified key.
  /// </summary>
  /// <param name="key">The key of the value to remove.</param>
  /// <returns>A task that represents the asynchronous removal operation.</returns>
  ValueTask DeleteAsync(string key);
}