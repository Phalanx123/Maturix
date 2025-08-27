namespace Maturix.Abstractions.Clients;

/// <summary>
/// Factory for creating <see cref="IMaturixClient"/> instances.
/// </summary>
public interface IMaturixClientFactory
{
    /// <summary>
    /// Creates a new <see cref="IMaturixClient"/> instance.
    /// </summary>
    /// <returns></returns>
    IMaturixClient Create();
    /// <summary>
    /// Creates a new <see cref="IMaturixClient"/> instance scoped to a specific location.
    /// </summary>
    /// <param name="locationId"></param>
    /// <returns></returns>
    IMaturixClient CreateForLocation(string locationId);
}