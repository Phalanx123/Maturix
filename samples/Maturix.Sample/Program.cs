using System;
using System.Net.Http;
using System.Threading.Tasks;
using Maturix;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Maturix.Sample
{
    /// <summary>
    /// Entry point for the sample console application demonstrating use of
    /// <see cref="MaturixClient"/>.  
    /// This example shows how to configure the client and request quality
    /// reports and production unit dashboards. Replace the placeholders with
    /// your own API key, location ID and production ID to exercise the API.
    /// </summary>
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            // Replace with your actual API key, location ID and production ID.
            const string apiKey = "YOUR_API_KEY";
            const string locationId = "LocationID";
            const string productionId = "ProductionID";

            // Create the options. You can change BaseUrl if Maturix provides a
            // different endpoint or for testing.
            var options = new MaturixClientOptions
            {
                ApiKey = apiKey,
                // BaseUrl defaults to "https://app.maturix.com/api/api.php".
            };

            // It is best practice to reuse HttpClient throughout the
            // application. Here we instantiate it with the base address from
            // options.
            using var httpClient = new HttpClient
            {
                BaseAddress = new Uri(options.BaseUrl)
            };

            // Use a null logger so the sample doesn't produce logging output.
            var logger = NullLogger<MaturixClient>.Instance;
            var client = new MaturixClient(httpClient, options, logger);

            Console.WriteLine("Fetching quality reports...");
            var reportsResult = await client.GetQualityReportsAsync(locationId);
            reportsResult.Switch(
                reports =>
                {
                    Console.WriteLine($"Received {reports.Count} quality reports:\n");
                    foreach (var report in reports)
                    {
                        Console.WriteLine($"Report {report.Id} – Production: {report.ProductionId} – Compound: {report.Compound}");
                    }
                },
                error => Console.WriteLine($"Error fetching reports: {error}")
            );

            Console.WriteLine("\nFetching production dashboard...");
            var dashboardResult = await client.GetProductionUnitDashboardAsync(productionId);
            dashboardResult.Switch(
                dashboard =>
                {
                    var stats = dashboard.Stats;
                    if (stats != null)
                    {
                        Console.WriteLine($"Production {stats.ProductionId}: Current strength {stats.CurrentStrength} at temperature {stats.CurrentTemp}°C");
                    }
                    else
                    {
                        Console.WriteLine("No stats available.");
                    }
                    if (dashboard.SensorData != null)
                    {
                        Console.WriteLine($"Received {dashboard.SensorData.Count} sensor data points.");
                    }
                },
                error => Console.WriteLine($"Error fetching dashboard: {error}")
            );
        }
    }
}