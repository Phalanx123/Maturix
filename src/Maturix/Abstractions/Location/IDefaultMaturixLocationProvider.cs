namespace Maturix.Abstractions.Location;

/// <summary>
/// Provides the default physical LocationId (plant/site) for the current execution/user context.
/// Implementations may use any storage (in-memory, DB, claims, etc).
/// </summary>
public interface IDefaultMaturixLocationProvider
{
    /// <summary>Return the default LocationId if known; otherwise null.</summary>
    string? GetDefaultLocationId();
}