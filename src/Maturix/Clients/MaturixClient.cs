using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Maturix.Abstractions.Clients;
using Maturix.Abstractions.Location;
using Maturix.Exceptions;
using Maturix.Helpers;
using Maturix.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OneOf;

namespace Maturix.Clients;

/// <inheritdoc />
public class MaturixClient : IMaturixClient
{
    private readonly HttpClient _http;
    private readonly ILogger<MaturixClient> _logger;
    private readonly MaturixClientOptions _options;
    private readonly IDefaultMaturixLocationProvider _defaultLocation;
    private string? _boundLocationId;

    /// <summary>
    /// Initializes a new instance of the <see cref="MaturixClient"/> class.
    /// </summary>
    /// <param name="http"></param>
    /// <param name="options"></param>
    /// <param name="logger"></param>
    /// <param name="defaultLocation"></param>
    /// <exception cref="ArgumentException"></exception>
    public MaturixClient(HttpClient http, IOptions<MaturixClientOptions> options, ILogger<MaturixClient> logger, IDefaultMaturixLocationProvider defaultLocation)
    {
        _http = http;
        _logger = logger;
        _defaultLocation = defaultLocation;
        _options = options.Value;

        if (string.IsNullOrWhiteSpace(_options.BaseUrl))
            throw new ArgumentException("BaseUrl must be provided", nameof(options));

        if (_http.BaseAddress == null)
            _http.BaseAddress = new Uri(_options.BaseUrl);
    }
    
    private string ResolveRequiredLocation()
    {
        var effective = _boundLocationId
                        ?? _defaultLocation.GetDefaultLocationId()
                        ?? _options.DefaultLocationId;

        if (string.IsNullOrWhiteSpace(effective))
            throw new MissingLocationException(); // custom exception

        return effective;
    }

    /// <inheritdoc />
    public IMaturixClient ForLocation(string locationId)
    {
        if (string.IsNullOrWhiteSpace(locationId))
            throw new ArgumentException("LocationId is required", nameof(locationId));

        var clone = new MaturixClient(_http, Options.Create(_options), _logger, _defaultLocation)
        {
            _boundLocationId = locationId
        };
        return clone;
    }
    
    
    /// <inheritdoc />
    public async Task<OneOf<IReadOnlyList<QualityReport>, ApiError>> GetQualityReportsAsync(CancellationToken ct = default)
    {
       
        if (string.IsNullOrWhiteSpace(_options.ApiKey))
            return new ApiError(401, "API key is missing");
        var loc = ResolveRequiredLocation();
        var result = await ApiHelper.GetAsync<QualityReportEnvelope>(
            _http,
            _logger,
            function: "QualityReports",
            apiKey: _options.ApiKey!,
            locationId: loc,
            ct: ct);

        return result.Match<OneOf<IReadOnlyList<QualityReport>, ApiError>>(
            ok => ok.QualityReports?.AsReadOnly() ?? [],
            err => err
        );
    }

    /// <inheritdoc />
    public async Task<OneOf<ProductionUnit, ApiError>> GetProductionUnitAsync(string productionId, CancellationToken ct = default)
    {  
     
        if (string.IsNullOrWhiteSpace(_options.ApiKey))
            return new ApiError(401, "API key is missing");
        if (string.IsNullOrWhiteSpace(productionId))
            throw new ArgumentException("Production ID is required", nameof(productionId));
        var loc = ResolveRequiredLocation();
        var result = await ApiHelper.GetAsync<ProductionUnit>(
            _http, _logger, "ProductionUnitDashboard", _options.ApiKey!,
            locationId: loc,
            extraParams: [new KeyValuePair<string, string>("ProductionID", productionId)],
            ct: ct);
        return result.Match<OneOf<ProductionUnit, ApiError>>(
            ok => ok,
            err => err
        );
    }

    /// <inheritdoc />
    public async Task<OneOf<IReadOnlyList<Sensor>, ApiError>> GetSensorsAsync(CancellationToken ct = default)
    {
   
        if (string.IsNullOrWhiteSpace(_options.ApiKey))
            return new ApiError(401, "API key is missing");
        var loc = ResolveRequiredLocation();
        var result = await ApiHelper.GetAsync<SensorsEnvelope>(
            _http, _logger, "LocationSensors", _options.ApiKey!,   locationId: loc,
            ct: ct);

        return result.Match<OneOf<IReadOnlyList<Sensor>, ApiError>>(
            ok => ok.Sensors?.AsReadOnly() ?? [],
            err => err
        );
    }

    /// <inheritdoc />
    public async Task<OneOf<IReadOnlyList<SensorProductionData>, ApiError>> GetSensorProductionData(CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(_boundLocationId))
            throw  new MissingLocationException();
        if (string.IsNullOrWhiteSpace(_options.ApiKey))
            return new ApiError(401, "API key is missing");

        var result = await ApiHelper.GetAsync<SensorProductionEnvelope>(
            _http, _logger, "CurrentProductionUnits", _options.ApiKey!,
            ct: ct);
        return result.Match<OneOf<IReadOnlyList<SensorProductionData>, ApiError>>(
            ok => ok.ProductionData?.AsReadOnly() ?? [],
            err => err
        );
    }
}
