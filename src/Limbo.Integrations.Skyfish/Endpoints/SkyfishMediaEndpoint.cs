using Limbo.Integrations.Skyfish.Options.Media;
using Limbo.Integrations.Skyfish.Responses.Media;

namespace Limbo.Integrations.Skyfish.Endpoints {

    public class SkyfishMediaEndpoint {

        public SkyfishHttpService Service { get; }

        public SkyfishMediaRawEndpoint Raw => Service.Client.Media;

        public SkyfishMediaEndpoint(SkyfishHttpService service) {
            Service = service;
        }

        #region Member methods

        /// <summary>
        /// Returns the tags (Exif data) of the media with the specified <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The ID of the media.</param>
        /// <returns>An instance of <see cref="SkyfishMediaTagsResponse"/> representing the response.</returns>
        public SkyfishMediaTagsResponse GetTags(int id) {
            return new SkyfishMediaTagsResponse(Raw.GetTags(id));
        }


        /// <summary>
        /// Returns the tags (Exif data) of the media identified by the specified <paramref name="options"/>.
        /// </summary>
        /// <param name="options">The options for the request to the API.</param>
        /// <returns>An instance of <see cref="SkyfishMediaTagsResponse"/> representing the response.</returns>
        public SkyfishMediaTagsResponse GetTags(SkyfishGetMediaTagsOptions options) {
            return new SkyfishMediaTagsResponse(Raw.GetTags(options));
        }

        #endregion

    }

}