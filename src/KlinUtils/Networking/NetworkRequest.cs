namespace KlinUtils.Networking;

/// <summary>
/// Helps with the build blocks for a network request.
/// </summary>
/// <param name="RequestType">Request method.</param>
/// <param name="Url">An instance of KlinUtils.Networking.IUrlBuilder.</param>
/// <param name="Headers">An array of key/value pair of the request headers.</param>
/// <param name="Payload">Specifies the request payload, i.e: data being sent in case of POST or PUT request.</param>
/// <param name="HttpClientName">The IHttpClientFactory name as registered in your DI.</param>
/// <param name="Timeout">Specifies the timeout after which the request throws timeout exception.</param>
public record struct NetworkRequest(
    RequestType RequestType,
    string Url,
    (string Key, string Value)[]? Headers = null,
    object? Payload = null,
    string? HttpClientName = null,
    TimeSpan? Timeout = null);