using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Maturix.Clients;
using Maturix.Models;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using Xunit;
using Maturix.Abstractions.Location;
using Maturix.Exceptions; // for MissingLocationException

namespace Maturix.Tests
{
    public class MaturixClientTests
    {
        // --- Test doubles -----------------------------------------------------

        private sealed class TestDefaultLocationProvider : IDefaultMaturixLocationProvider
        {
            private readonly string? _locationId;
            public TestDefaultLocationProvider(string? locationId) => _locationId = locationId;
            public string? GetDefaultLocationId() => _locationId;
        }

        private sealed class CapturingHandler : HttpMessageHandler
        {
            private readonly HttpResponseMessage _toReturn;
            public HttpRequestMessage? LastRequest { get; private set; }

            public CapturingHandler(HttpResponseMessage toReturn) => _toReturn = toReturn;

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                LastRequest = request;
                return Task.FromResult(_toReturn);
            }
        }

        // --- Helpers ----------------------------------------------------------

        private static (HttpClient http, CapturingHandler handler) CreateHttpClient(HttpResponseMessage response)
        {
            var handler = new CapturingHandler(response);
            var http = new HttpClient(handler)
            {
                // BaseAddress must be set because the client builds relative URLs like "?f=..."
                BaseAddress = new Uri("https://app.maturix.com/api/api.php")
            };
            return (http, handler);
        }

        private static MaturixClient CreateClient(HttpClient http, string? providerDefault = "47", string? optionsDefault = null)
        {
            var options = Options.Create(new MaturixClientOptions
            {
                ApiKey = "test",
                BaseUrl = "https://app.maturix.com/api/api.php",
                DefaultLocationId = optionsDefault
            });

            var provider = new TestDefaultLocationProvider(providerDefault);
            return new MaturixClient(http, options, NullLogger<MaturixClient>.Instance, provider);
        }

        // --- Tests ------------------------------------------------------------

        [Fact]
        public async Task GetQualityReportsAsync_ReturnsReports_And_IncludesLocationInQuery()
        {
            var json = """
            {
              "status": 200,
              "statusmsg": "OK",
              "data": [
                {
                  "ID": "1",
                  "Name": "Test Report",
                  "ProductionID": "Prod1",
                  "WorkstationID": "WS1",
                  "ZoneID": "Z1",
                  "Status": "3"
                }
              ]
            }
            """;

            var (httpClient, handler) = CreateHttpClient(
                new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(json) }
            );
            // Provide a default Location via provider → no exception, and LocationID must be appended.
            var client = CreateClient(httpClient, providerDefault: "Location1");

            var result = await client.GetQualityReportsAsync();

            Assert.True(result.IsT0, "Expected success variant");
            var reports = result.AsT0;
            Assert.Single(reports);
            Assert.Equal("1", reports[0].Id);
            Assert.Equal("Prod1", reports[0].ProductionId);

            // Verify query contains LocationID and key
            Assert.NotNull(handler.LastRequest);
            var uri = handler.LastRequest!.RequestUri!.ToString();
            Assert.Contains("?f=QualityReports", uri);
            Assert.Contains("&key=test", uri);
            Assert.Contains("&LocationID=Location1", uri);
        }

        [Fact]
        public async Task GetQualityReportsAsync_ReturnsError_WhenHttpStatusIsNotSuccess()
        {
            var (httpClient, _) = CreateHttpClient(
                new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = "Internal Server Error" }
            );
            var client = CreateClient(httpClient, providerDefault: "Location1");

            var result = await client.GetQualityReportsAsync();

            Assert.True(result.IsT1, "Expected error variant");
            var error = result.AsT1;
            Assert.Equal((int)HttpStatusCode.InternalServerError, error.StatusCode);
            Assert.Contains("Internal Server Error", error.Message);
        }

        [Fact]
        public async Task GetQualityReportsAsync_ReturnsError_WhenApiStatusIsNot200()
        {
            var json = """{ "status": 400, "statusmsg": "Bad Request", "data": null }""";
            var (httpClient, _) = CreateHttpClient(
                new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(json) }
            );
            var client = CreateClient(httpClient, providerDefault: "Location1");

            var result = await client.GetQualityReportsAsync();

            Assert.True(result.IsT1, "Expected error variant");
            var error = result.AsT1;
            Assert.Equal(400, error.StatusCode);
            Assert.Equal("Bad Request", error.Message);
        }

        [Fact]
        public async Task GetQualityReportsAsync_ThrowsMissingLocation_WhenNoLocationCanBeResolved()
        {
            // Arrange: no provider default, no options default, not bound via ForLocation()
            var (httpClient, _) = CreateHttpClient(
                new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("""{ "status":200, "statusmsg":"OK", "data": [] }""") }
            );
            var client = CreateClient(httpClient, providerDefault: null, optionsDefault: null);

            // Act + Assert
            await Assert.ThrowsAsync<MissingLocationException>(() => client.GetQualityReportsAsync());
        }
        [Fact]
        public async Task GetProductionUnitDashboardAsync_ReturnsDashboard_And_IncludesLocationInQuery()
        {
            var json = """
                       {
                         "status": 200,
                         "statusmsg": "OK",
                         "data": {
                           "Stats": {
                             "ProductionID": "ProdX",
                             "CurrentStrength": 20.5,
                             "CurrentTemp": 18.3
                           },
                           "Sensordata": [
                             { "unix": 1, "temp": 18.3, "strength": null, "maturity_seconds": null }
                           ]
                         }
                       }
                       """;

            var (httpClient, handler) = CreateHttpClient(
                new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(json) }
            );

            // Provide a default Location so the resolver succeeds
            var client = CreateClient(httpClient, providerDefault: "Location1", optionsDefault: null);

            var result = await client.GetProductionUnitAsync("ProdX");

            Assert.True(result.IsT0, "Expected success variant");
            var dashboard = result.AsT0;
            Assert.NotNull(dashboard.Stats);
            Assert.Equal("ProdX", dashboard.Stats!.ProductionId);
            Assert.Equal(20.5, dashboard.Stats.CurrentStrength);
            Assert.Equal(18.3, dashboard.Stats.CurrentTemp);
            Assert.NotNull(dashboard.SensorData);
            Assert.Single(dashboard.SensorData!);
            Assert.Equal(1, dashboard.SensorData![0].Unix);

            // Verify LocationID IS present
            Assert.NotNull(handler.LastRequest);
            var uri = handler.LastRequest!.RequestUri!.ToString();
            Assert.Contains("?f=ProductionUnitDashboard", uri);
            Assert.Contains("&key=test", uri);
            Assert.Contains("&LocationID=Location1", uri);   // <- required
            Assert.Contains("&ProductionID=ProdX", uri);
        }

    }
}
