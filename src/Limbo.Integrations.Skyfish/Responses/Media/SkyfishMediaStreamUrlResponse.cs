using Limbo.Integrations.Skyfish.Models.Media;
using Skybrud.Essentials.Http;

namespace Limbo.Integrations.Skyfish.Responses.Media;

/// <summary>
/// Class representing a response with information about the stream URL of a Skyfish media.
/// </summary>
public class SkyfishMediaStreamUrlResponse : SkyfishResponse<SkyfishMediaStreamUrl> {

    /// <summary>
    /// Initializes a new instance based on the specified <paramref name="response"/>.
    /// </summary>
    /// <param name="response">An instance of <see cref="IHttpResponse"/> representing the raw response.</param>
    public SkyfishMediaStreamUrlResponse(IHttpResponse response) : base(response) {
        Body = ParseJsonObject(response.Body, SkyfishMediaStreamUrl.Parse)!;
    }

}