using Limbo.Integrations.Skyfish.Models.Media;
using Skybrud.Essentials.Http;

namespace Limbo.Integrations.Skyfish.Responses.Media {

    /// <summary>
    /// Class representing the response listing the tags (Exif data) of media.
    /// </summary>
    public class SkyfishMediaTagsResponse : SkyfishResponse<SkyfishMediaTags> {

        /// <summary>
        /// Initializes a new instance based on the specified <paramref name="response"/>.
        /// </summary>
        /// <param name="response">An instance of <see cref="IHttpResponse"/> representing the raw response.</param>
        public SkyfishMediaTagsResponse(IHttpResponse response) : base(response) {
            Body = ParseJsonObject(response.Body, SkyfishMediaTags.Parse)!;
        }

    }

}