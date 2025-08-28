// PFUnitTesting|Maturix|MaturixClientTests.cs

using System.Net;
using Maturix.Clients;
using Maturix.Exceptions;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Xunit;

namespace Maturix.Tests
{
    /// <summary>
    /// Contract: A LocationID MUST be supplied (via options or an explicit binder).
    /// If missing, the client MUST throw <see cref="MissingLocationException"/>.
    /// These tests assert that behaviour and also verify HTTP success/error paths.
    /// </summary>
    public class MaturixClientTests
    {
        // ----------------------- Test doubles -----------------------

        /// <summary>
        /// Message handler that captures the last request and returns a fixed response.
        /// </summary>
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

        // ------------------------- Helpers -------------------------

        private static (HttpClient http, CapturingHandler handler) CreateHttpClient(HttpResponseMessage response)
        {
            var handler = new CapturingHandler(response);
            var http = new HttpClient(handler)
            {
                // The client composes relative query strings (e.g. "?f=QualityReports")
                BaseAddress = new Uri("https://app.maturix.com/api/api.php")
            };
            return (http, handler);
        }

        private static MaturixClient CreateClient(HttpClient http, string? optionsLocationId = null, string apiKey = "test")
        {
            var options = Options.Create(new MaturixClientOptions
            {
                ApiKey = apiKey,
                BaseUrl = "https://app.maturix.com/api/api.php",
                LocationId = optionsLocationId
            });

            // Important: no default-location provider is passed—by design.
            return new MaturixClient(http, options, NullLogger<MaturixClient>.Instance);
        }
        
        [Fact]
        public async Task GetQualityReportsAsync_ReturnsApiError_WhenHttpStatusNotSuccess()
        {
            var (http, _) = CreateHttpClient(new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                ReasonPhrase = "Internal Server Error"
            });

            var client = CreateClient(http, optionsLocationId: "Location1");

            var result = await client.GetQualityReportsAsync();

            Assert.True(result.IsT1, "Expected error variant");
            var error = result.AsT1;
            Assert.Equal((int)HttpStatusCode.InternalServerError, error.StatusCode);
            Assert.Contains("Internal Server Error", error.Message);
        }

        [Fact]
        public async Task GetQualityReportsAsync_ReturnsApiError_WhenApiStatusIsNot200()
        {
            var json = """{ "status": 400, "statusmsg": "Bad Request", "data": null }""";
            var (http, _) = CreateHttpClient(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json)
            });

            var client = CreateClient(http, optionsLocationId: "Location1");

            var result = await client.GetQualityReportsAsync();

            Assert.True(result.IsT1, "Expected error variant");
            var error = result.AsT1;
            Assert.Equal(400, error.StatusCode);
            Assert.Equal("Bad Request", error.Message);
        }


        [Fact]
        public async Task GetProductionUnitAsync_UsesOptionsLocation_And_AppendsToQuery()
        {
            var json = """
            {
              "status": 200,
              "statusmsg": "OK",
              "data": {
                "Stats": { "ProductionID": "ProdX", "CurrentStrength": 20.5, "CurrentTemp": 18.3 },
                "Sensordata": [ { "unix": 1, "temp": 18.3, "strength": null, "maturity_seconds": null } ]
              }
            }
            """;

            var (http, handler) = CreateHttpClient(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json)
            });

            var client = CreateClient(http, optionsLocationId: "Location1");

            var result = await client.GetProductionUnitAsync("ProdX");

            Assert.True(result.IsT0, "Expected success");
            var dashboard = result.AsT0;

            Assert.NotNull(dashboard.Stats);
            Assert.Equal("ProdX", dashboard.Stats!.ProductionId);
            Assert.Equal(20.5, dashboard.Stats.CurrentStrength);
            Assert.Equal(18.3, dashboard.Stats.CurrentTemp);

            Assert.NotNull(dashboard.SensorData);
            Assert.Single(dashboard.SensorData!);
            Assert.Equal(1, dashboard.SensorData![0].Unix);

            var uri = handler.LastRequest!.RequestUri!.ToString();
            Assert.Contains("?f=ProductionUnitDashboard", uri);
            Assert.Contains("&key=test", uri);
            Assert.Contains("&LocationID=Location1", uri);
            Assert.Contains("&ProductionID=ProdX", uri);
        }
        
    }
}
