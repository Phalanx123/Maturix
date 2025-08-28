using System.Net;
using System.Text;
using System.Text.Json;
using Maturix.Helpers;
using Maturix.Models;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Xunit;

namespace Maturix.Tests.TestHelpers;

/// <summary>
/// Tests for <see cref="ApiHelper"/>.
/// </summary>
public class ApiHelperTests
{
    private readonly Mock<ILogger> _mockLogger;
    private readonly Mock<HttpMessageHandler> _mockHttpHandler;
    private readonly HttpClient _httpClient;
    private readonly MaturixClientOptions _options;

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiHelperTests"/> class.
    /// </summary>
    public ApiHelperTests()
    {
        _mockLogger = new Mock<ILogger>();
        _mockHttpHandler = new Mock<HttpMessageHandler>();
        _httpClient = new HttpClient(_mockHttpHandler.Object)
        {
            BaseAddress = new Uri("https://api.example.com/")
        };
        _options = new MaturixClientOptions
        {
            ApiKey = "test-key",
            LocationId = "test-location"
        };
    }

    [Fact]
    public async Task GetAsync_SuccessfulResponse_ReturnsTypedData()
    {
        // Arrange
        var expectedData = new TestDataModel { Id = 123, Name = "Test" };
        var responseJson = JsonSerializer.Serialize(new
        {
            status = 200,
            statusmsg = "OK",
            data = expectedData
        });

        SetupHttpResponse(HttpStatusCode.OK, responseJson);

        // Act
        var result = await ApiHelper.GetAsync<TestDataModel>(_httpClient, _mockLogger.Object, "testFunction", _options);

        // Assert
        Assert.True(result.IsT0);
        var data = result.AsT0;
        Assert.Equal(123, data.Id);
        Assert.Equal("Test", data.Name);
    }

    [Fact]
    public async Task GetAsync_SuccessEnvelopeResponse_ReturnsSuccessEnvelope()
    {
        // Arrange
        var responseJson = JsonSerializer.Serialize(new
        {
            status = 200,
            statusmsg = "OK",
            data = new { Success = 1 }
        });

        SetupHttpResponse(HttpStatusCode.OK, responseJson);

        // Act
        var result = await ApiHelper.GetAsync<SuccessEnvelope>(_httpClient, _mockLogger.Object, "testFunction", _options);

        // Assert
        Assert.True(result.IsT0);
        var envelope = result.AsT0;
        Assert.Equal(1, envelope.Success);
    }

    [Fact]
    public async Task GetAsync_ErrorResponseWithStringData_ReturnsApiError()
    {
        // Arrange
        var responseJson = JsonSerializer.Serialize(new
        {
            status = 400,
            statusmsg = "Bad Request",
            data = "Invalid parameters provided"
        });

        SetupHttpResponse(HttpStatusCode.OK, responseJson);

        // Act
        var result = await ApiHelper.GetAsync<TestDataModel>(_httpClient, _mockLogger.Object, "testFunction", _options);

        // Assert
        Assert.True(result.IsT1);
        var error = result.AsT1;
        Assert.Equal(400, error.StatusCode);
        Assert.Equal("Invalid parameters provided", error.Message);
    }

    [Fact]
    public async Task GetAsync_ErrorResponseWithoutData_ReturnsApiErrorWithStatusMessage()
    {
        // Arrange
        var responseJson = JsonSerializer.Serialize(new
        {
            status = 500,
            statusmsg = "Internal Server Error",
            data = (object?)null
        });

        SetupHttpResponse(HttpStatusCode.OK, responseJson);

        // Act
        var result = await ApiHelper.GetAsync<TestDataModel>(_httpClient, _mockLogger.Object, "testFunction", _options);

        // Assert
        Assert.True(result.IsT1);
        var error = result.AsT1;
        Assert.Equal(500, error.StatusCode);
        Assert.Equal("Internal Server Error", error.Message);
    }

    [Fact]
    public async Task GetAsync_HttpErrorResponse_ReturnsApiError()
    {
        // Arrange
        SetupHttpResponse(HttpStatusCode.NotFound, "Not Found");

        // Act
        var result = await ApiHelper.GetAsync<TestDataModel>(_httpClient, _mockLogger.Object, "testFunction", _options);

        // Assert
        Assert.True(result.IsT1);
        var error = result.AsT1;
        Assert.Equal(404, error.StatusCode);
        Assert.Equal("Not Found", error.Message);
    }

    [Fact]
    public async Task GetAsync_MissingApiKey_ReturnsApiError()
    {
        // Arrange
        var invalidOptions = new MaturixClientOptions { ApiKey = "", LocationId = "test-location" };

        // Act
        var result = await ApiHelper.GetAsync<TestDataModel>(_httpClient, _mockLogger.Object, "testFunction", invalidOptions);

        // Assert
        Assert.True(result.IsT1);
        var error = result.AsT1;
        Assert.Equal(401, error.StatusCode);
        Assert.Equal("API key is missing", error.Message);
    }

    [Fact]
    public async Task GetAsync_MissingLocationId_ReturnsApiError()
    {
        // Arrange
        var invalidOptions = new MaturixClientOptions { ApiKey = "test-key", LocationId = "" };

        // Act
        var result = await ApiHelper.GetAsync<TestDataModel>(_httpClient, _mockLogger.Object, "testFunction", invalidOptions);

        // Assert
        Assert.True(result.IsT1);
        var error = result.AsT1;
        Assert.Equal(401, error.StatusCode);
        Assert.Equal("LocationID is missing", error.Message);
    }

    [Fact]
    public async Task GetAsync_EmptyResponse_ReturnsApiError()
    {
        // Arrange - empty string causes JSON deserialization to fail
        SetupHttpResponse(HttpStatusCode.OK, "");

        // Act
        var result = await ApiHelper.GetAsync<TestDataModel>(_httpClient, _mockLogger.Object, "testFunction", _options);

        // Assert
        Assert.True(result.IsT1);
        var error = result.AsT1;
        Assert.Equal(-1, error.StatusCode);
        Assert.Contains("JSON token", error.Message);
    }

    [Fact]
    public async Task GetAsync_NullResponse_ReturnsApiError()
    {
        // Arrange
        SetupHttpResponse(HttpStatusCode.OK, "null");

        // Act
        var result = await ApiHelper.GetAsync<TestDataModel>(_httpClient, _mockLogger.Object, "testFunction", _options);

        // Assert
        Assert.True(result.IsT1);
        var error = result.AsT1;
        Assert.Equal(-1, error.StatusCode);
        Assert.Equal("Empty response", error.Message);
    }

    [Fact]
    public async Task GetAsync_InvalidJson_ReturnsApiError()
    {
        // Arrange
        SetupHttpResponse(HttpStatusCode.OK, "invalid json{");

        // Act
        var result = await ApiHelper.GetAsync<TestDataModel>(_httpClient, _mockLogger.Object, "testFunction", _options);

        // Assert
        Assert.True(result.IsT1);
        var error = result.AsT1;
        Assert.Equal(-1, error.StatusCode);
        Assert.NotEmpty(error.Message); 
    }

    [Fact]
    public async Task GetAsync_CancellationRequested_ThrowsTaskCanceledException()
    {
        // Arrange
        using var cts = new CancellationTokenSource();
        await cts.CancelAsync();

        _mockHttpHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.Is<CancellationToken>(ct => ct.IsCancellationRequested))
            .ThrowsAsync(new OperationCanceledException());

        // Act & Assert
        await Assert.ThrowsAsync<TaskCanceledException>(
            () => ApiHelper.GetAsync<TestDataModel>(_httpClient, _mockLogger.Object, "testFunction", _options, ct: cts.Token));
    }

    [Theory]
    [InlineData("QualityReports", "test-key", "loc-123", "?f=QualityReports&key=test-key&LocationID=loc-123")]
    [InlineData("GetData", "api-key-456", null, "?f=GetData&key=api-key-456")]
    [InlineData("Special Function", "key with spaces", "location", "?f=Special%20Function&key=key%20with%20spaces&LocationID=location")]
    public void BuildSignedUrl_VariousInputs_ReturnsCorrectUrl(string function, string apiKey, string? locationId, string expected)
    {
        // Act
        var result = ApiHelper.BuildSignedUrl(function, apiKey, locationId);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void BuildSignedUrl_WithExtraParams_IncludesAllParameters()
    {
        // Arrange
        var extraParams = new[]
        {
            new KeyValuePair<string, string>("param1", "value1"),
            new KeyValuePair<string, string>("param2", "value with spaces")
        };

        // Act
        var result = ApiHelper.BuildSignedUrl("TestFunc", "key", "loc", extraParams);

        // Assert
        Assert.Equal("?f=TestFunc&key=key&LocationID=loc&param1=value1&param2=value%20with%20spaces", result);
    }

    private void SetupHttpResponse(HttpStatusCode statusCode, string content)
    {
        var response = new HttpResponseMessage(statusCode)
        {
            Content = new StringContent(content, Encoding.UTF8, "application/json")
        };

        _mockHttpHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);
    }
}

/// <summary>
/// A simple test data model.
/// </summary>
public class TestDataModel
{
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    public string Name { get; set; } = string.Empty;
}
