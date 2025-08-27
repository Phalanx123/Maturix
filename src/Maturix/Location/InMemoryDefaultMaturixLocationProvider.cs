using System.Threading;
using Maturix.Abstractions.Location;

namespace Maturix.Location;

/// <summary>
/// Library-default provider: per-async-flow storage using AsyncLocal.
/// Host apps can replace this with a DB/claims-backed provider.
/// </summary>
public sealed class InMemoryDefaultMaturixLocationProvider : IDefaultMaturixLocationProvider
{
    private static readonly AsyncLocal<string?> Current = new();

    /// <summary>
    /// Sets the current default LocationId for this async flow.
    /// </summary>
    /// <param name="locationId"></param>
    public static void Set(string? locationId) => Current.Value = locationId;

    /// <inheritdoc />
    public string? GetDefaultLocationId() => Current.Value;
}