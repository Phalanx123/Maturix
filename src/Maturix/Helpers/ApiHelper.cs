using System.Text;
using System.Text.Json;
using Maturix.Models;
using Microsoft.Extensions.Logging;
using OneOf;

namespace Maturix.Helpers;

/// <summary>
/// HTTP helpers for Maturix API requests.
/// Centralises URL signing (API key + optional LocationID) and envelope parsing.
/// </summary>
internal static class ApiHelper
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = true
    };

    private static string Enc(string? v) => Uri.EscapeDataString(v ?? string.Empty);

    /// <summary>
    /// Builds a signed query string:
    /// ?f={function}&key={apiKey}[&LocationID=...][&extra...]
    /// </summary>
    /// <param name="function">Maturix function name (e.g., "QualityReports").</param>
    /// <param name="apiKey">API key (required).</param>
    /// <param name="locationId">Optional physical LocationID.</param>
    /// <param name="extraParams">Optional extra query parameters.</param>
    internal static string BuildSignedUrl(
        string function,
        string apiKey,
        string? locationId = null,
        IEnumerable<KeyValuePair<string, string>>? extraParams = null)
    {
        if (string.IsNullOrWhiteSpace(function))
            throw new ArgumentException("Function is required", nameof(function));
        if (string.IsNullOrWhiteSpace(apiKey))
            throw new ArgumentException("API key is required", nameof(apiKey));

        var sb = new StringBuilder($"?f={Enc(function)}&key={Enc(apiKey)}");

        if (!string.IsNullOrWhiteSpace(locationId))
            sb.Append("&LocationID=").Append(Enc(locationId));

        if (extraParams is null) return sb.ToString();

        foreach (var kv in extraParams)
        {
            if (string.IsNullOrWhiteSpace(kv.Key)) continue;
            sb.Append('&').Append(Enc(kv.Key)).Append('=').Append(Enc(kv.Value));
        }

        return sb.ToString();
    }

    /// <summary>
    /// Executes a GET request using a signed Maturix URL (function + key + optional LocationID + extras).
    /// Deserialises into RequestResponse{T} and returns Data or ApiError.
    /// </summary>
    public static async Task<OneOf<T, ApiError>> GetAsync<T>(
        HttpClient http,
        ILogger logger,
        string function,
        string apiKey,
        string? locationId = null,
        IEnumerable<KeyValuePair<string, string>>? extraParams = null,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(http);
        ArgumentNullException.ThrowIfNull(logger);

        var url = BuildSignedUrl(function, apiKey, locationId, extraParams);

        try
        {
            using var resp = await http.GetAsync(url, ct).ConfigureAwait(false);
            if (!resp.IsSuccessStatusCode)
            {
                logger.LogError("Maturix API returned non-success {StatusCode} for {Url}", (int)resp.StatusCode, url);
                return new ApiError((int)resp.StatusCode, resp.ReasonPhrase ?? "HTTP error");
            }

            await using var stream = await resp.Content.ReadAsStreamAsync(ct).ConfigureAwait(false);
            var envelope = await JsonSerializer.DeserializeAsync<RequestResponse<T>>(stream, JsonOptions, ct)
                .ConfigureAwait(false);

            if (envelope is null)
                return new ApiError(-1, "Empty response");
            if (envelope.Status != 200)
                return new ApiError(envelope.Status, envelope.StatusMessage ?? "Unknown error");
            if (envelope.Data is null)
                return new ApiError(-1, "Missing data");

            return envelope.Data;
        }
        catch (OperationCanceledException)
        {
            logger.LogInformation("Operation cancelled for {Url}", url);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception occurred calling {Url}", url);
            return new ApiError(-1, ex.Message);
        }
    }
}
