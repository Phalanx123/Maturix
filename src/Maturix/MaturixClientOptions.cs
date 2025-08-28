using Maturix.Clients;

namespace Maturix
{
    /// <summary>
    /// Configuration settings for <see cref="MaturixClient"/>.  
    /// These options specify the base URL of the API and the API key used for
    /// authentication. You should register or instantiate this class and
    /// supply it to the client at construction time.
    /// </summary>
    public class MaturixClientOptions
    {
        /// <summary>
        /// The base URL used for all API calls. The default points to the
        /// production Maturix endpoint. You can override this for testing or if
        /// Maturix provides a different base URL in the future.
        /// </summary>
        public string? BaseUrl { get; set; } = "https://app.maturix.com/api/api.php";

        /// <summary>
        /// Your API key. This is required for all API calls. If this value is
        /// null or empty, calls will fail with an <see cref="ApiError"/>.
        /// </summary>
        public string? ApiKey { get; set; }
      
        /// <summary>
        /// Library-level fallback only. Typical flows rely on IDefaultMaturixLocationProvider.
        /// </summary>
        public string? LocationId { get; set; }
    }
}