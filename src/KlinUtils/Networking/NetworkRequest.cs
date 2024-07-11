namespace KlinUtils.Networking;

public record struct NetworkRequest(
    RequestType RequestType,
    string Url,
    (string Key, string Value)[]? Headers = null,
    object? Payload = null,
    string? HttpClientName = null,
    TimeSpan? Timeout = null);