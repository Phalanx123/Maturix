using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Maturix.Models;
using OneOf;

namespace Maturix
{
    /// <summary>
    /// A concrete implementation of <see cref="IMaturixClient"/> that uses
    /// <see cref="HttpClient"/> to communicate with the Maturix API.
    /// </summary>
    public class MaturixClient : IMaturixClient
    {
        private readonly HttpClient _httpClient;
        private readonly MaturixClientOptions _options;
        private readonly ILogger<MaturixClient> _logger;

        /// <summary>
        /// Initialises a new instance of the <see cref="MaturixClient"/> class.
        /// </summary>
        /// <param name="httpClient">The <see cref="HttpClient"/> used to send requests. This client should be configured with a valid base address.</param>
        /// <param name="options">The configuration options containing the API key and base URL.</param>
        /// <param name="logger">A logger used to record informational and error messages.</param>
        /// <exception cref="ArgumentNullException">Thrown if any dependency is null.</exception>
        public MaturixClient(HttpClient httpClient, MaturixClientOptions options, ILogger<MaturixClient> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            if (string.IsNullOrWhiteSpace(_options.BaseUrl))
            {
                throw new ArgumentException("BaseUrl must be provided", nameof(options));
            }

            // Ensure the HttpClient has the correct base address. If the caller already set
            // BaseAddress we respect it; otherwise we set it from options.
            if (_httpClient.BaseAddress == null)
            {
                _httpClient.BaseAddress = new Uri(_options.BaseUrl);
            }
        }

        /// <inheritdoc/>
        public async Task<OneOf<IReadOnlyList<QualityReport>, ApiError>> GetQualityReportsAsync(
            string locationId,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(locationId))
            {
                throw new ArgumentException("Location ID must be provided", nameof(locationId));
            }

            if (string.IsNullOrWhiteSpace(_options.ApiKey))
            {
                return new ApiError((int)HttpStatusCode.Unauthorized, "API key is missing");
            }

            var url = $"?f=QualityReports&key={Uri.EscapeDataString(_options.ApiKey)}&LocationID={Uri.EscapeDataString(locationId)}";

            try
            {
                using var response = await _httpClient.GetAsync(url, cancellationToken).ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Maturix API returned non-success status code {StatusCode} when fetching quality reports", (int)response.StatusCode);
                    return new ApiError((int)response.StatusCode, response.ReasonPhrase ?? "HTTP error");
                }

                var stream = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var parsed = await JsonSerializer.DeserializeAsync<QualityReportsResponse>(stream, options, cancellationToken).ConfigureAwait(false);

                if (parsed == null)
                {
                    _logger.LogError("Received empty or unparseable response when fetching quality reports");
                    return new ApiError(-1, "Unable to parse API response");
                }

                if (parsed.Status != 200)
                {
                    _logger.LogError("Maturix API returned status {Status} with message '{Message}'", parsed.Status, parsed.StatusMessage);
                    return new ApiError(parsed.Status, parsed.StatusMessage ?? "Unknown error");
                }

                var reports = (IReadOnlyList<QualityReport>?)parsed.Data?.QualityReports ?? Array.Empty<QualityReport>();
                return reports;
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Operation cancelled while fetching quality reports");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An exception occurred while fetching quality reports");
                return new ApiError(-1, ex.Message);
            }
        }

        /// <inheritdoc/>
        public async Task<OneOf<ProductionUnitDashboard, ApiError>> GetProductionUnitDashboardAsync(
            string productionId,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(productionId))
            {
                throw new ArgumentException("Production ID must be provided", nameof(productionId));
            }

            if (string.IsNullOrWhiteSpace(_options.ApiKey))
            {
                return new ApiError((int)HttpStatusCode.Unauthorized, "API key is missing");
            }

            var url = $"?f=ProductionUnitDashboard&key={Uri.EscapeDataString(_options.ApiKey)}&ProductionID={Uri.EscapeDataString(productionId)}";

            try
            {
                using var response = await _httpClient.GetAsync(url, cancellationToken).ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Maturix API returned non-success status code {StatusCode} when fetching production dashboard", (int)response.StatusCode);
                    return new ApiError((int)response.StatusCode, response.ReasonPhrase ?? "HTTP error");
                }

                var stream = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var parsed = await JsonSerializer.DeserializeAsync<ProductionUnitDashboardResponse>(stream, options, cancellationToken).ConfigureAwait(false);
                if (parsed == null)
                {
                    _logger.LogError("Received empty or unparseable response when fetching production dashboard");
                    return new ApiError(-1, "Unable to parse API response");
                }
                if (parsed.Status != 200)
                {
                    _logger.LogError("Maturix API returned status {Status} with message '{Message}' for production dashboard", parsed.Status, parsed.StatusMessage);
                    return new ApiError(parsed.Status, parsed.StatusMessage ?? "Unknown error");
                }
                var dashboard = parsed.Data;
                if (dashboard == null)
                {
                    _logger.LogWarning("Maturix API returned a success status but no data for production dashboard");
                    return new ApiError(-1, "Missing data in API response");
                }
                return dashboard;
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Operation cancelled while fetching production dashboard");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An exception occurred while fetching production dashboard");
                return new ApiError(-1, ex.Message);
            }
        }
    }
}