using System.Net;

using KlinUtils.DiscriminatedUnion;

namespace KlinUtils.Networking;

/// <summary>
/// Provides a clean, simple and efficient mechanism to make REST API calls.
/// </summary>
public interface INetworkClient
{
    /// <summary>
    /// Gets the status code of a request.
    /// </summary>
    HttpStatusCode StatusCode { get; }

    /// <summary>
    /// Sends a REST API request with the request configuration.
    /// </summary>
    /// <typeparam name="T">The expected return type from the API call.</typeparam>
    /// <param name="request">An instance of the KlinUtils.Networking.NetworkRequest.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Result<typeparamref name="T"/>.</returns>
    ValueTask<Result<T>> SendRequestAsync<T>(
        NetworkRequest request,
        CancellationToken cancellationToken);

    /// <summary>
    /// Sends a REST API request with the request configuration.
    /// </summary>
    /// <typeparam name="T">The expected return type from the API call.</typeparam>
    /// <param name="request">An instance of the KlinUtils.Networking.NetworkRequest.</param>
    /// <param name="jsonTypeInfo">Serialization-related metadata.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Result<typeparamref name="T"/>.</returns>
    ValueTask<Result<T>> SendRequestAsync<T>(
        NetworkRequest request,
        System.Text.Json.Serialization.Metadata.JsonTypeInfo? jsonTypeInfo = default,
        CancellationToken? cancellationToken = default);
}