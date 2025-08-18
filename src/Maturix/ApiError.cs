using System;
using System.Net;

namespace Maturix
{
    /// <summary>
    /// Represents an error returned by the Maturix API.  
    /// If the API returns a non‑success HTTP status code or if the JSON response
    /// indicates a failure, an instance of this class will be returned in lieu
    /// of the expected data.
    /// </summary>
    public sealed class ApiError
    {
        /// <summary>
        /// Creates a new <see cref="ApiError"/> with the provided status code and message.
        /// </summary>
        /// <param name="statusCode">The HTTP status code or the code reported by the API.</param>
        /// <param name="message">A human readable message describing the error.</param>
        public ApiError(int statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message ?? throw new ArgumentNullException(nameof(message));
        }

        /// <summary>
        /// Gets the numeric status code associated with the error.  
        /// When a network call fails the HTTP status code is used. When the API
        /// returns a JSON payload with a non‑200 <c>status</c> field, that value
        /// is exposed here.
        /// </summary>
        public int StatusCode { get; }

        /// <summary>
        /// Gets a description of the error. This message is suitable for
        /// logging or user display.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Returns a string representation of the error including the code and
        /// message.
        /// </summary>
        public override string ToString() => $"Status {StatusCode}: {Message}";
    }
}