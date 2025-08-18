# Maturix API Client Library

This repository contains a .NET library for interacting with the **Maturix** API.  
The goal of this package is to provide strongly typed, well‑structured access to
the endpoints exposed by Maturix so that applications can retrieve quality
reports and production unit statistics without worrying about the underlying
HTTP calls or JSON parsing.

## Projects

The solution is organised into three projects:

| Project | Description |
|---|---|
| **src/Maturix** | Class library containing the client, options and models for consuming the Maturix API. |
| **samples/Maturix.Sample** | A simple console application demonstrating how to use the client to fetch reports. |
| **tests/Maturix.Tests** | Unit tests covering the behaviour of the client. The tests use `Moq` to simulate HTTP responses and are namespaced under `PFUnitTesting`. |

## Getting Started

1. **Reference the library** – Add a project reference to `src/Maturix/Maturix.csproj` or install the compiled NuGet package (if published).  
2. **Configure options** – Create a `MaturixClientOptions` instance specifying your API key and (optionally) a custom base URL.  
3. **Instantiate the client** – Provide an `HttpClient`, the options and an `ILogger<MaturixClient>` when constructing `MaturixClient`.  
4. **Call the API** – Use `GetQualityReportsAsync` or `GetProductionUnitDashboardAsync` to fetch data. Each method returns a `OneOf` value containing either the requested data or an `ApiError` describing what went wrong.

## Example

```csharp
using Maturix;
using Microsoft.Extensions.Logging.Abstractions;

var httpClient = new HttpClient
{
    BaseAddress = new Uri("https://app.maturix.com/api/api.php")
};

var options = new MaturixClientOptions
{
    ApiKey = "YOUR_API_KEY"
};

var client = new MaturixClient(httpClient, options, NullLogger<MaturixClient>.Instance);

// Fetch quality reports for a specific location
var reportsResult = await client.GetQualityReportsAsync("LocationID");
reportsResult.Switch(
    reports =>
    {
        foreach (var r in reports)
        {
            Console.WriteLine($"Report {r.Id} – Production: {r.ProductionId}");
        }
    },
    error => Console.WriteLine($"Error: {error.Message} (status {error.StatusCode})")
);
```

## Notes

* The library uses `System.Text.Json` for JSON deserialisation.
* Errors are logged through `ILogger`. You can inject any implementation, for example the
  default logging from `Microsoft.Extensions.Logging` or `NullLogger` for no output.
* All methods are asynchronous and return `Task` for non‑blocking I/O.
