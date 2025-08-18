using System.Text.Json.Serialization;

namespace Maturix.Models
{
    /// <summary>
    /// Represents the top‑level response for the ProductionUnitDashboard endpoint.
    /// </summary>
    internal class ProductionUnitDashboardResponse
    {
        [JsonPropertyName("status")]
        public int Status { get; set; }

        [JsonPropertyName("statusmsg")]
        public string? StatusMessage { get; set; }

        [JsonPropertyName("data")]
        public ProductionUnitDashboard? Data { get; set; }
    }
}