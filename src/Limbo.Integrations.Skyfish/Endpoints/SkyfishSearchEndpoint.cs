using Limbo.Integrations.Skyfish.Options.Search;
using Limbo.Integrations.Skyfish.Responses.Search;

namespace Limbo.Integrations.Skyfish.Endpoints;

/// <summary>
/// Class representing the implementation of the <strong>Search</strong> endpoint.
/// </summary>
public class SkyfishSearchEndpoint {

    #region Properties

    /// <summary>
    /// Gets a reference to the Skyfish service.
    /// </summary>
    public SkyfishHttpService Service { get; }

    /// <summary>
    /// Gets a reference to the raw endpoint.
    /// </summary>
    public SkyfishSearchRawEndpoint Raw => Service.Client.Search;

    #endregion

    #region Constructors

    internal SkyfishSearchEndpoint(SkyfishHttpService service) {
        Service = service;
    }

    #endregion

    #region Member methods

    /// <summary>
    /// Returns a list of media items matching the specified search <paramref name="options"/>.
    /// </summary>
    /// <param name="options">The options for the request to the API.</param>
    /// <returns>An instance of <see cref="SkyfishSearchResponse"/> representing the response.</returns>
    public SkyfishSearchResponse Search(SkyfishSearchOptions options) {
        return new SkyfishSearchResponse(Raw.Search(options));
    }

    #endregion

}