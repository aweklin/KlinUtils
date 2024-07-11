using System.Net;

using FluentAssertions;

using KlinUtils.DiscriminatedUnion;
using KlinUtils.Networking;

using NSubstitute;

namespace KlinUtils.UnitTests.Networking;

public class NetworkClientTests
{
    private readonly INetworkClient _networkClient;
    private readonly CancellationToken _cancellationToken = default;

    public NetworkClientTests()
    {
        IHttpClientFactory httpClientFactory = Substitute.For<IHttpClientFactory>();
        _networkClient = new NetworkClient(httpClientFactory);
    }

    [Fact]
    public async Task SendRequestAsync_ShouldReturnUrlRequiredError_WhenUrlProvidedDoesNotExists()
    {
        // given
        NetworkRequest networkRequest = new(RequestType.Get, string.Empty);
        Error expectedError = NetworkErrors.UrlRequired;
        int expectedErrorCount = 1;

        // when
        Result<object> actualResult = await _networkClient.SendRequestAsync<object>(networkRequest, _cancellationToken);

        actualResult.HasError.Should().BeTrue();
        actualResult.Errors.Count.Should().Be(expectedErrorCount);
        actualResult.Errors.First().Should().BeEquivalentTo(expectedError);
    }

    [Fact]
    public async Task SendRequestAsync_ShouldReturnInvalidUrlError_WhenUrlProvidedIsInvalid()
    {
        // given
        NetworkRequest networkRequest = new(RequestType.Get, "web.abc");
        Error expectedError = NetworkErrors.InvalidUrl;
        int expectedErrorCount = 1;

        // when
        Result<object> actualResult = await _networkClient.SendRequestAsync<object>(networkRequest, _cancellationToken);

        actualResult.HasError.Should().BeTrue();
        actualResult.Errors.Count.Should().Be(expectedErrorCount);
        actualResult.Errors.First().Should().BeEquivalentTo(expectedError);
    }

    [Fact]
    public async Task SendRequestAsync_ShouldReturnPayloadNotAllowedError_WhenPayloadIsProvidedForGetOrDeleteRequest()
    {
        // given
        NetworkRequest networkRequest = new(
            RequestType.Get,
            "https://google.com",
            Payload: new { id = 1, name = "Test" });
        Error expectedError = NetworkErrors.PayloadNotAllowed;
        int expectedErrorCount = 1;

        // when
        Result<object> actualResult = await _networkClient.SendRequestAsync<object>(networkRequest, _cancellationToken);

        actualResult.HasError.Should().BeTrue();
        actualResult.Errors.Count.Should().Be(expectedErrorCount);
        actualResult.Errors.First().Should().BeEquivalentTo(expectedError);
    }

    [Fact]
    public async Task SendRequestAsync_ShouldReturnNotFoundError_WhenUrlProvidedDoesNotExists()
    {
        // given
        string invalidUrl = "https://googlex.com";
        NetworkRequest networkRequest = new(RequestType.Get, invalidUrl);
        Error expectedError = NetworkErrors.UrlNotFound;
        ValueTask<Result<object>> expectedResult = GetFailureResultFrom<object>(expectedError);
        int expectedErrorCount = 1;
        HttpStatusCode expectedStatusCode = HttpStatusCode.NotFound;
        INetworkClient networkClient = Substitute.For<INetworkClient>();

        networkClient
            .SendRequestAsync<object>(networkRequest, _cancellationToken)
            .Returns(expectedResult);
        networkClient
            .StatusCode
            .Returns(expectedStatusCode);

        // when
        Result<object> actualResult = await networkClient.SendRequestAsync<object>(networkRequest, _cancellationToken);

        actualResult.HasError.Should().BeTrue();
        actualResult.Errors.Count.Should().Be(expectedErrorCount);
        actualResult.Errors.First().Should().BeEquivalentTo(expectedError);
        networkClient.StatusCode.Should().Be(expectedStatusCode);
    }

    private static async ValueTask<Result<T>> GetFailureResultFrom<T>(Error error)
    {
        await ValueTask.CompletedTask;
        return Result.Failure<T>(error);
    }
}