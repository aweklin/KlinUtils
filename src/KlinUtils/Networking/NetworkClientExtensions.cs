namespace KlinUtils.Networking;

internal static class NetworkClientExtensions
{
    public static HttpClient AddHeaders(this HttpClient httpClient, NetworkRequest request)
    {
        httpClient.DefaultRequestHeaders.Clear();
        if (request.Headers is not null)
        {
            foreach (var (key, value) in request.Headers)
            {
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation(key, value);
            }
        }

        return httpClient;
    }

    public static HttpClient AddTimeout(this HttpClient httpClient, NetworkRequest request)
    {
        if (request.Timeout is not null)
        {
            httpClient.Timeout = request.Timeout.Value;
        }

        return httpClient;
    }

    public static string ToStringValue(this RequestType requestType) =>
        requestType switch
        {
            RequestType.Get => nameof(RequestType.Get),
            RequestType.Post => nameof(RequestType.Post),
            RequestType.Put => nameof(RequestType.Put),
            RequestType.Delete => nameof(RequestType.Delete),
            _ => throw new ArgumentOutOfRangeException(nameof(requestType)),
        };
}