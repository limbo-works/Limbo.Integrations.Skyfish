using System.Net;
using Skybrud.Essentials.Http;
using Limbo.Integrations.Skyfish.Exceptions;

namespace Limbo.Integrations.Skyfish.Responses;

/// <summary>
/// Class representing a response from the Skyfish API.
/// </summary>
public class SkyfishResponse : HttpResponseBase {

    /// <summary>
    /// Initializes a new instance based on the specified <paramref name="response"/>.
    /// </summary>
    /// <param name="response">The instance of <see cref="IHttpResponse"/> representing the raw response.</param>
    public SkyfishResponse(IHttpResponse response) : base(response) {
        if (response.StatusCode == HttpStatusCode.OK) return;
        if (response.StatusCode == HttpStatusCode.Created) return;
        throw new SkyfishHttpException(response);
    }

}

/// <summary>
/// Class representing a response from the Skyfish API.
/// </summary>
public class SkyfishResponse<T> : SkyfishResponse {

    /// <summary>
    /// /// Gets the body of the response.
    /// </summary>
    public T Body { get; protected set; } = default!;

    /// <summary>
    /// Initializes a new instance based on the specified <paramref name="response"/>.
    /// </summary>
    /// <param name="response">The instance of <see cref="IHttpResponse"/> representing the raw response.</param>
    public SkyfishResponse(IHttpResponse response) : base(response) { }

}