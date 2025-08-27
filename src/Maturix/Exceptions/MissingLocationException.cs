using System;

namespace Maturix.Exceptions;

/// <inheritdoc />
public sealed class MissingLocationException : Exception
{
    /// <inheritdoc />
    public MissingLocationException()
        : base("Location ID is required but could not be resolved.") { }
}