using System.Text.Json.Serialization;

namespace Maturix.Models
{
    /// <summary>
    /// Represents the top‑level response for the QualityReports endpoint.  
    /// The API wraps the actual list of reports inside a <c>data</c> object.
    /// </summary>
    internal class RequestResponse<T>
    {
        [JsonPropertyName("status")]
        public int Status { get; set; }

        [JsonPropertyName("statusmsg")]
        public string? StatusMessage { get; set; }

        [JsonPropertyName("data")]
        public T? Data { get; set; }
    }

    internal class QualityReportsData
    {
        [JsonPropertyName("QualityReports")]
        public List<QualityReport>? QualityReports { get; set; }
    }
}