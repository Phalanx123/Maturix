using Microsoft.Extensions.Options;

namespace Maturix.Tests.TestHelpers;

internal static class OptionsHelper
{
    public static IOptions<T> From<T>(T value) where T : class => Options.Create(value);
}