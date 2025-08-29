using Maturix.Abstractions.Clients;
using Maturix.Helpers;
using Maturix.Models;
using Maturix.Models.Envelopes;
using Maturix.Models.ProductionPlan;
using Maturix.Models.Requests;
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


    /// <summary>
    /// Initializes a new instance of the <see cref="MaturixClient"/> class.
    /// </summary>
    /// <param name="http"></param>
    /// <param name="options"></param>
    /// <param name="logger"></param>
    /// <exception cref="ArgumentException"></exception>
    public MaturixClient(HttpClient http, IOptions<MaturixClientOptions> options, ILogger<MaturixClient> logger)
    {
        _http = http;
        _logger = logger;
    
        _options = options.Value;

        if (string.IsNullOrWhiteSpace(_options.BaseUrl))
            throw new ArgumentException("BaseUrl must be provided", nameof(options));

        if (_http.BaseAddress == null)
            _http.BaseAddress = new Uri(_options.BaseUrl);
    }

    /// <inheritdoc />
    public IMaturixClient ForLocation(string locationId)
    {
        if (string.IsNullOrWhiteSpace(locationId))
            throw new ArgumentException("LocationId is required", nameof(locationId));

        var newOptions = new MaturixClientOptions
        {
            ApiKey = _options.ApiKey,
            BaseUrl = _options.BaseUrl,
            LocationId = locationId
        };

        var clone = new MaturixClient(_http, Options.Create(newOptions), _logger);
        return clone;
    }
    
    /// <inheritdoc />
    public async Task<OneOf<IReadOnlyList<QualityReport>, ApiError>> GetQualityReportsAsync(CancellationToken ct = default)
    {
        var result = await ApiHelper.GetAsync<QualityReportEnvelope>(
            _http,
            _logger,
            function: MaturixFunctions.QualityReports,
            options: _options,
            ct: ct);

        return result.Match<OneOf<IReadOnlyList<QualityReport>, ApiError>>(
            ok => ok.QualityReports?.AsReadOnly() ?? new List<QualityReport>().AsReadOnly(),
            err => err
        );
    }

    /// <inheritdoc />
    public async Task<OneOf<ProductionUnit, ApiError>> GetProductionUnitAsync(string productionId, CancellationToken ct = default)
    {  
        if (string.IsNullOrWhiteSpace(productionId))
            throw new ArgumentException("Production ID is required", nameof(productionId));

        var result = await ApiHelper.GetAsync<ProductionUnit>(
            _http, _logger, MaturixFunctions.ProductionUnitDashboard, _options,
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
        var result = await ApiHelper.GetAsync<SensorsEnvelope>(
            _http, _logger, MaturixFunctions.LocationSensors, _options,
            ct: ct);

        return result.Match<OneOf<IReadOnlyList<Sensor>, ApiError>>(
            ok => ok.Sensors?.AsReadOnly() ?? new List<Sensor>().AsReadOnly(),
            err => err
        );
    }

    /// <inheritdoc />
    public async Task<OneOf<IReadOnlyList<SensorProductionData>, ApiError>> GetSensorProductionData(CancellationToken ct = default)
    {
        var result = await ApiHelper.GetAsync<SensorProductionEnvelope>(
            _http, _logger, MaturixFunctions.CurrentProductionUnits, _options,
            ct: ct);
        return result.Match<OneOf<IReadOnlyList<SensorProductionData>, ApiError>>(
            ok => ok.ProductionData?.AsReadOnly() ?? new List<SensorProductionData>().AsReadOnly(),
            err => err
        );
    }

    /// <inheritdoc />
    public async Task<OneOf<IReadOnlyList<Compound>, ApiError>> GetCompoundsAsync(CancellationToken ct = default)
    {
        var result = await ApiHelper.GetAsync<CompoundEnvelope>(
            _http, _logger, MaturixFunctions.LocationCompounds, _options,
            ct: ct);
        return result.Match<OneOf<IReadOnlyList<Compound>, ApiError>>(
            ok => ok.Compounds?.AsReadOnly() ?? new List<Compound>().AsReadOnly(),
            err => err
        );
    }

    /// <inheritdoc />
    public async Task<OneOf<bool, ApiError>> NewProductionPlan(NewProductionPlanEntryRequest planEntryRequest, CancellationToken ct = default)
    {
        var result = await ApiHelper.GetAsync<SuccessEnvelope>(
            _http, _logger, MaturixFunctions.NewProductionPlanEntry, _options,
            extraParams: planEntryRequest.ToQueryParams() ,
            ct: ct);
        return result.Match<OneOf<bool, ApiError>>(
            _ => result.AsT0.Success == 1,
            err => err);
    }

    /// <inheritdoc />
    public async Task<OneOf<IReadOnlyList<ProductionPlanResponse>, ApiError>> GetProductionPlans(CancellationToken ct = default)
    {
        var result = await ApiHelper.GetAsync<ProductionPlanEnvelope>(
            _http, _logger, MaturixFunctions.LocationProductionPlan, _options,
            ct: ct);
        return result.Match<OneOf<IReadOnlyList<ProductionPlanResponse>, ApiError>>(
            ok => ok.ProductionPlans?.AsReadOnly() ?? new List<ProductionPlanResponse>().AsReadOnly(),
            err => err
        );
    }
}
