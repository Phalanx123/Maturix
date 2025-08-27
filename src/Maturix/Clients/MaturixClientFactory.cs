using System;
using System.Net.Http;
using Maturix.Abstractions.Clients;
using Maturix.Abstractions.Location;
using Maturix.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Maturix.Clients;

/// <inheritdoc />
public sealed class MaturixClientFactory : IMaturixClientFactory
{
    private readonly IHttpClientFactory _httpFactory;
    private readonly IOptions<MaturixClientOptions> _options;
    private readonly ILogger<MaturixClient> _logger;
    private readonly IDefaultMaturixLocationProvider _defaultLocation;

    /// <summary>
    /// Creates Client Factory
    /// </summary>
    /// <param name="httpFactory"></param>
    /// <param name="options"></param>
    /// <param name="logger"></param>
    /// <param name="defaultLocation"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public MaturixClientFactory(
        IHttpClientFactory httpFactory,
        IOptions<MaturixClientOptions> options,
        ILogger<MaturixClient> logger,
        IDefaultMaturixLocationProvider defaultLocation)
    {
        _httpFactory = httpFactory ?? throw new ArgumentNullException(nameof(httpFactory));
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _defaultLocation = defaultLocation ?? throw new ArgumentNullException(nameof(defaultLocation));
    }

    /// <inheritdoc />
    public IMaturixClient Create()
        => new MaturixClient(_httpFactory.CreateClient("Maturix"), _options, _logger, _defaultLocation);

    /// <inheritdoc />
    public IMaturixClient CreateForLocation(string locationId)
    {
        var client = Create();
        return client.ForLocation(locationId);
    }
}