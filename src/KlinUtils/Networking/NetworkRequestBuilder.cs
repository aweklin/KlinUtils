namespace KlinUtils.Networking;

/// <summary>
/// Provides a fluent interface to build a NetworkRequest object.
/// </summary>
public class NetworkRequestBuilder
{
    private static readonly Lazy<Implementer> _implementer = new(() => new());

    /// <summary>
    /// Instantiates an instance of the IRequestTypeBuilder.
    /// </summary>
    /// <returns>KlinUtils.Networking.IRequestTypeBuilder.</returns>
    public static IRequestTypeBuilder Create() => _implementer.Value;

    /// <summary>
    /// Specifies the request headers.
    /// </summary>
    /// <param name="headers">An array of key/value pair of the request headers.</param>
    /// <returns>KlinUtils.Networking.NetworkRequestBuilder.</returns>
    public NetworkRequestBuilder WithHeaders((string Key, string Value)[] headers)
    {
        _implementer.Value.NetworkRequest = _implementer.Value.NetworkRequest with { Headers = headers };
        return this;
    }

    /// <summary>
    /// Specifies the request payload, i.e: data being sent in case of POST or PUT request.
    /// </summary>
    /// <typeparam name="T">Generic object.</typeparam>
    /// <param name="payload">Data being sent along side the request.</param>
    /// <returns>KlinUtils.Networking.NetworkRequestBuilder.</returns>
    public NetworkRequestBuilder RequestBody<T>(T payload)
    {
        _implementer.Value.NetworkRequest = _implementer.Value.NetworkRequest with { Payload = payload };
        return this;
    }

    /// <summary>
    /// Specifies the timeout after which the request throws timeout exception.
    /// </summary>
    /// <param name="timeSpan">System.TimeSpan.</param>
    /// <returns>KlinUtils.Networking.NetworkRequestBuilder.</returns>
    public NetworkRequestBuilder TimesOutAfter(TimeSpan timeSpan)
    {
        _implementer.Value.NetworkRequest = _implementer.Value.NetworkRequest with { Timeout = timeSpan };
        return this;
    }

    /// <summary>
    /// Indicates that a named System.Net.Http.IHttpClientFactory is to be used for this request.
    /// </summary>
    /// <param name="httpClientName">The IHttpClientFactory name as registered in your DI.</param>
    /// <returns>System.Net.Http.IHttpClientFactory.</returns>
    public NetworkRequestBuilder Uses(string httpClientName)
    {
        _implementer.Value.NetworkRequest = _implementer.Value.NetworkRequest with { HttpClientName = httpClientName };
        return this;
    }

    /// <summary>
    /// Returns a fully built NetworkRequest object.
    /// </summary>
    /// <returns>An instance of KlinUtils.Networking.NetworkRequest.</returns>
    public NetworkRequest Build() => _implementer.Value.NetworkRequest;

    private class Implementer() : NetworkRequestBuilder, IRequestTypeBuilder, IUrlBuilder
    {
        internal NetworkRequest NetworkRequest { get; set; }
        public IUrlBuilder For(RequestType requestType)
        {
            NetworkRequest = NetworkRequest with
            {
                RequestType = requestType,
                Headers = [],
                HttpClientName = string.Empty,
                Payload = default!,
                Timeout = default!,
                Url = string.Empty,
            };
            return this;
        }

        /// <summary>
        /// Specifies the request url.
        ///     If you already specified the HttpClientName, no need to specify the full url, just provide the endpoint.
        /// </summary>
        /// <param name="url">A value, representing the full url or (uri) the endpoint.</param>
        /// <returns>An instance of KlinUtils.Networking.IUrlBuilder.</returns>
        public NetworkRequestBuilder WithUrl(string url)
        {
            NetworkRequest = NetworkRequest with { Url = url };
            return this;
        }
    }
}

/// <summary>
/// Used to initiate a KlinUtils.Networking.NetworkRequest object by providing the request type.
/// </summary>
public interface IRequestTypeBuilder
{
    /// <summary>
    /// Specifies the request method being used - GET, PUT, POST, DELETE.
    /// </summary>
    /// <param name="requestType">Request method.</param>
    /// <returns>An instance of KlinUtils.Networking.IUrlBuilder.</returns>
    IUrlBuilder For(RequestType requestType);
}

/// <summary>
/// Allows you to specify request url and eventually be able to set other optional parameters for your request.
/// </summary>
public interface IUrlBuilder
{
    /// <summary>
    /// Specify request uri for your request.
    ///     If you already specified the HttpClientName, no need to specify the full url.
    /// </summary>
    /// <param name="url">A value, representing the full url or (uri) the endpoint.</param>
    /// <returns>An instance of KlinUtils.Networking.IUrlBuilder.</returns>
    NetworkRequestBuilder WithUrl(string url);
}