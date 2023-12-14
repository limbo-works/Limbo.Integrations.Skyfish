using Skybrud.Essentials.Http;
using Limbo.Integrations.Skyfish.Models.Search;

namespace Limbo.Integrations.Skyfish.Responses.Search;

/// <summary>
/// Class representing the response of a media search.
/// </summary>
public class SkyfishSearchResponse : SkyfishResponse<SkyfishSearchResult> {

    /// <summary>
    /// Initializes a new instance based on the specified <paramref name="response"/>.
    /// </summary>
    /// <param name="response">An instance of <see cref="IHttpResponse"/> representing the raw response.</param>
    public SkyfishSearchResponse(IHttpResponse response) : base(response) {
        Body = ParseJsonObject(response.Body, SkyfishSearchResult.Parse)!;
    }

}