using System;
using System.Net;
using Skybrud.Essentials.Http;
using Skybrud.Essentials.Http.Exceptions;

namespace Limbo.Integrations.Skyfish.Exceptions;

/// <summary>
/// Class representing an exception/error returned by the Skyfish API.
/// </summary>
public class SkyfishHttpException : Exception, IHttpException {

    /// <summary>
    /// Gets a reference to the underlying <see cref="IHttpResponse"/>.
    /// </summary>
    public IHttpResponse Response { get; }

    /// <summary>
    /// Gets the HTTP status code returned by the Skyfish API.
    /// </summary>
    public HttpStatusCode StatusCode => Response.StatusCode;

    /// <summary>
    /// Initializes a new exception based on the specified <paramref name="response"/>.
    /// </summary>
    /// <param name="response">The instance of <see cref="IHttpResponse"/> representing the raw response.</param>
    public SkyfishHttpException(IHttpResponse response) : base($"Invalid response received from the Skyfish API (status: {(int) response.StatusCode})") {
        Response = response;
    }

}