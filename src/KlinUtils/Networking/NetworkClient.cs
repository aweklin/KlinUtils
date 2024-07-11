using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

using KlinUtils.DiscriminatedUnion;

namespace KlinUtils.Networking;

internal sealed class NetworkClient : INetworkClient
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    private static readonly MediaTypeWithQualityHeaderValue _applicationJson = new("application/json");
    private static readonly MediaTypeHeaderValue _applicationJsonHeader = new("application/json");

    public HttpStatusCode StatusCode { get; private set; }

    public NetworkClient(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
        _jsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true,
        };
    }

    public async ValueTask<Result<T>> SendRequestAsync<T>(
        NetworkRequest request,
        CancellationToken cancellationToken)
    {
        return await SendRequestAsync<T>(request, null, cancellationToken);
    }

    public async ValueTask<Result<T>> SendRequestAsync<T>(
        NetworkRequest request,
        System.Text.Json.Serialization.Metadata.JsonTypeInfo? jsonTypeInfo = default,
        CancellationToken? cancellationToken = default)
    {
        Result<T> validationResult = Validate<T>(request);
        if (validationResult.HasError)
        {
            await ValueTask.CompletedTask;
            return validationResult;
        }

        HttpClient httpClient = CreateHttpClient(request)
            .AddHeaders(request)
            .AddTimeout(request);

        string requestTypeString = request.RequestType.ToStringValue();
        HttpRequestMessage httpRequestMessage = new(
            method: new HttpMethod(requestTypeString),
            requestUri: request.Url);

        httpRequestMessage.Headers.Clear();
        httpRequestMessage.Headers.Accept.Add(_applicationJson);

        if (request.Payload is { })
        {
            MemoryStream memoryStream = new();
            if (jsonTypeInfo is not null)
            {
                await JsonSerializer.SerializeAsync(
                    memoryStream,
                    request.Payload,
                    jsonTypeInfo,
                    cancellationToken!.Value);
            }
            else
            {
                await JsonSerializer.SerializeAsync(
                    memoryStream,
                    value: request.Payload,
                    cancellationToken: cancellationToken!.Value);
            }

            memoryStream.Seek(0, SeekOrigin.Begin);

            using StreamContent streamContent = new(memoryStream);
            httpRequestMessage.Content = streamContent;
            streamContent.Headers.ContentType = _applicationJsonHeader;

            return await ProcessRequestAsync<T>(
                    httpClient,
                    httpRequestMessage,
                    cancellationToken!.Value);
        }

        return await ProcessRequestAsync<T>(
                    httpClient,
                    httpRequestMessage,
                    cancellationToken!.Value);
    }

    private async ValueTask<Result<T>> ProcessRequestAsync<T>(
        HttpClient httpClient,
        HttpRequestMessage httpRequestMessage,
        CancellationToken cancellationToken)
    {
        HttpResponseMessage response = await httpClient.SendAsync(
                    request: httpRequestMessage,
                    completionOption: HttpCompletionOption.ResponseHeadersRead,
                    cancellationToken: cancellationToken);
        StatusCode = response.StatusCode;

        if (!response.IsSuccessStatusCode)
        {
            string errorResponseContent = await response.Content.ReadAsStringAsync(cancellationToken);
            string responseText = string.IsNullOrWhiteSpace(errorResponseContent) ?
                response.ReasonPhrase ?? string.Empty :
                errorResponseContent;
            Error errorResponse = new(response.StatusCode.ToString(), responseText);
            return Result.Failure<T>(errorResponse);
        }

        Stream responseContent = await response.Content.ReadAsStreamAsync(cancellationToken);
        T? responseValue = await JsonSerializer.DeserializeAsync<T>(
            utf8Json: responseContent,
            options: _jsonSerializerOptions,
            cancellationToken: cancellationToken);

        return Result.Success(responseValue ?? default!);
    }

    private HttpClient CreateHttpClient(NetworkRequest request) =>
        request.HttpClientName switch
        {
            null => _httpClientFactory.CreateClient(),
            _ => _httpClientFactory.CreateClient(request.HttpClientName),
        };

    private static Result<T> Validate<T>(NetworkRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Url))
        {
            return Result.Failure<T>(NetworkErrors.UrlRequired);
        }

        try
        {
            if (string.IsNullOrWhiteSpace(request.HttpClientName)
                && request.Url.AsSpan()[..4] != "http")
            {
                _ = new Uri(request.Url);
            }

            RequestType[] requestTypesValidToHavePayload = [RequestType.Get, RequestType.Delete];
            if (request.Payload is { } && requestTypesValidToHavePayload.Contains(request.RequestType))
            {
                return Result.Failure<T>(NetworkErrors.PayloadNotAllowed);
            }
        }
        catch
        {
            return Result.Failure<T>(NetworkErrors.InvalidUrl);
        }

        return Result.Success((T)default!);
    }
}