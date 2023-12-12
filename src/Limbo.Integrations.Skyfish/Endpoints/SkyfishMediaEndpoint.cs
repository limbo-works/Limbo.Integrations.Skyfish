using Limbo.Integrations.Skyfish.Options.Media;
using Limbo.Integrations.Skyfish.Responses.Media;

namespace Limbo.Integrations.Skyfish.Endpoints {

    /// <summary>
    /// Class representing the implementation of the <strong>Media</strong> endpoint.
    /// </summary>
    public class SkyfishMediaEndpoint {

        #region Properties

        /// <summary>
        /// Gets a reference to the Skyfish service.
        /// </summary>
        public SkyfishHttpService Service { get; }

        /// <summary>
        /// Gets a reference to the raw endpoint.
        /// </summary>
        public SkyfishMediaRawEndpoint Raw => Service.Client.Media;

        #endregion

        #region Constructors

        internal SkyfishMediaEndpoint(SkyfishHttpService service) {
            Service = service;
        }

        #endregion

        #region Member methods

        /// <summary>
        /// Returns a list of tags (Exif data) of the media with the specified <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The ID of the media.</param>
        /// <returns>An instance of <see cref="SkyfishMediaTagsResponse"/> representing the response.</returns>
        public SkyfishMediaTagsResponse GetTags(int id) {
            return new SkyfishMediaTagsResponse(Raw.GetTags(id));
        }

        /// <summary>
        /// Returns a list of tags (Exif data) of the media identified by the specified <paramref name="options"/>.
        /// </summary>
        /// <param name="options">The options for the request to the API.</param>
        /// <returns>An instance of <see cref="SkyfishMediaTagsResponse"/> representing the response.</returns>
        public SkyfishMediaTagsResponse GetTags(SkyfishGetMediaTagsOptions options) {
            return new SkyfishMediaTagsResponse(Raw.GetTags(options));
        }

        #endregion

    }

}