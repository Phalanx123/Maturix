namespace Maturix.Abstractions;

/// <summary>
/// Provides a stable, explicit mapping from a request DTO to query-string pairs.
/// </summary>
public interface IQueryParamSerializable
{
    /// <summary>
    /// Converts the current instance to a collection of key-value pairs suitable for use as query parameters in a URL.
    /// </summary>
    /// <returns></returns>
    IEnumerable<KeyValuePair<string, string>> ToQueryParams();
}