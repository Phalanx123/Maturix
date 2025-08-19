using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Moq.Protected;
using Xunit;

namespace Maturix.Tests
{
    /// <summary>
    /// Tests for <see cref="MaturixClient"/>.  
    /// These tests use a mocked <see cref="HttpMessageHandler"/> to control
    /// responses from the Maturix API without performing real HTTP calls.
    /// </summary>
    public class MaturixClientTests
    {
        private static HttpClient CreateHttpClient(HttpResponseMessage response)
        {
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(response)
                .Verifiable();
            return new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("https://app.maturix.com/api/api.php")
            };
        }

        [Fact]
        public async Task GetQualityReportsAsync_ReturnsReports_WhenApiReturnsSuccess()
        {
            // Arrange
            var json = @"{\n\""status\"": 200,\n\""statusmsg\"": \""OK\"",\n\""data\"": {\n \""QualityReports\"": [\n  {\n   \""ID\"": \""1\"",\n   \""Name\"": \""Test Report\"",\n   \""ProductionID\"": \""Prod1\"",\n   \""WorkstationID\"": \""WS1\"",\n   \""ZoneID\"": \""Z1\"",\n   \""Status\"": \""3\""\n  }\n ]\n}\n}";
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json)
            };
            var httpClient = CreateHttpClient(response);
            var options = new MaturixClientOptions { ApiKey = "dummy" };
            var client = new MaturixClient(httpClient, options, NullLogger<MaturixClient>.Instance);

            // Act
            var result = await client.GetQualityReportsAsync("Location1");

            // Assert
            Assert.True(result.IsT0, "Result should be a success variant");
            var reports = result.AsT0;
            Assert.Single(reports);
            Assert.Equal("1", reports[0].Id);
            Assert.Equal("Prod1", reports[0].ProductionId);
        }

        [Fact]
        public async Task GetQualityReportsAsync_ReturnsError_WhenHttpStatusIsNotSuccess()
        {
            // Arrange
            var response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                ReasonPhrase = "Internal Server Error"
            };
            var httpClient = CreateHttpClient(response);
            var options = new MaturixClientOptions { ApiKey = "dummy" };
            var client = new MaturixClient(httpClient, options, NullLogger<MaturixClient>.Instance);

            // Act
            var result = await client.GetQualityReportsAsync("Location1");

            // Assert
            Assert.True(result.IsT1, "Result should be an error variant");
            var error = result.AsT1;
            Assert.Equal((int)HttpStatusCode.InternalServerError, error.StatusCode);
            Assert.Contains("Internal Server Error", error.Message);
        }

        [Fact]
        public async Task GetQualityReportsAsync_ReturnsError_WhenApiStatusIsNot200()
        {
            // Arrange
            var json = @"{\""status\"": 400,\""statusmsg\"": \""Bad Request\"",\""data\"": null}";
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json)
            };
            var httpClient = CreateHttpClient(response);
            var options = new MaturixClientOptions { ApiKey = "dummy" };
            var client = new MaturixClient(httpClient, options, NullLogger<MaturixClient>.Instance);

            // Act
            var result = await client.GetQualityReportsAsync("Location1");

            // Assert
            Assert.True(result.IsT1, "Result should be an error variant");
            var error = result.AsT1;
            Assert.Equal(400, error.StatusCode);
            Assert.Equal("Bad Request", error.Message);
        }

        [Fact]
        public async Task GetProductionUnitDashboardAsync_ReturnsDashboard_WhenApiReturnsSuccess()
        {
            // Arrange
            var json = @"{\n  \""status\"": 200,\n  \""statusmsg\"": \""OK\"",\n  \""data\"": {\n    \""Stats\"": {\n      \""ProductionID\"": \""ProdX\"",\n      \""CurrentStrength\"": 20.5,\n      \""CurrentTemp\"": 18.3\n    },\n    \""Sensordata\"": [ { \""unix\"": 1, \""temp\"": 18.3, \""strength\"": null, \""maturity_seconds\"": null } ]\n  }\n}";
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json)
            };
            var httpClient = CreateHttpClient(response);
            var options = new MaturixClientOptions { ApiKey = "dummy" };
            var client = new MaturixClient(httpClient, options, NullLogger<MaturixClient>.Instance);

            // Act
            var result = await client.GetProductionUnitDashboardAsync("ProdX");

            // Assert
            Assert.True(result.IsT0, "Result should be a success variant");
            var dashboard = result.AsT0;
            Assert.NotNull(dashboard.Stats);
            Assert.Equal("ProdX", dashboard.Stats!.ProductionId);
            Assert.Equal(20.5, dashboard.Stats.CurrentStrength);
            Assert.Equal(18.3, dashboard.Stats.CurrentTemp);
            Assert.NotNull(dashboard.SensorData);
            Assert.Single(dashboard.SensorData);
            Assert.Equal(1, dashboard.SensorData![0].Unix);
        }
    }
}