using System;
using Limbo.Integrations.Skyfish.Http;
using Limbo.Integrations.Skyfish.Options.Search;
using Skybrud.Essentials.Http;

namespace Limbo.Integrations.Skyfish.Endpoints;

/// <summary>
/// Class representing the implementation of the raw <strong>Search</strong> endpoint.
/// </summary>
public class SkyfishSearchRawEndpoint {

    #region Properties

    /// <summary>
    /// Gets a reference to the underlying HTTP client.
    /// </summary>
    public SkyfishHttpClient Client { get; }

    #endregion

    #region Constructors

    internal SkyfishSearchRawEndpoint(SkyfishHttpClient client) {
        Client = client;
    }

    #endregion

    #region Member methods

    /// <summary>
    /// Returns a list of media items matching the specified search <paramref name="options"/>.
    /// </summary>
    /// <param name="options">The options for the request to the API.</param>
    /// <returns>An instance of <see cref="IHttpResponse"/> representing the raw response.</returns>
    public IHttpResponse Search(SkyfishSearchOptions options) {
        if (options == null) throw new ArgumentNullException(nameof(options));
        return Client.GetResponse(options);
    }

    #endregion

}