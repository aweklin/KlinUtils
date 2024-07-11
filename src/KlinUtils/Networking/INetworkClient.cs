using System.Net;
using System.Text.Json.Serialization;

using KlinUtils.DiscriminatedUnion;

namespace KlinUtils.Networking;

public interface INetworkClient
{
    HttpStatusCode StatusCode { get; }

    ValueTask<Result<T>> SendRequestAsync<T>(
        NetworkRequest request,
        CancellationToken cancellationToken);

    ValueTask<Result<T>> SendRequestAsync<T>(
        NetworkRequest request,
        System.Text.Json.Serialization.Metadata.JsonTypeInfo? jsonTypeInfo = default,
        CancellationToken? cancellationToken = default);
}