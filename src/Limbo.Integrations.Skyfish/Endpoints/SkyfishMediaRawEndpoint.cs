using System;
using Limbo.Integrations.Skyfish.Http;
using Limbo.Integrations.Skyfish.Options.Media;
using Skybrud.Essentials.Http;

namespace Limbo.Integrations.Skyfish.Endpoints {

    /// <summary>
    /// Class representing the implementation of the raw <strong>Media</strong> endpoint.
    /// </summary>
    public class SkyfishMediaRawEndpoint {

        #region Properties

        /// <summary>
        /// Gets a reference to the underlying HTTP client.
        /// </summary>
        public SkyfishHttpClient Client { get; }

        #endregion

        #region Constructors

        internal SkyfishMediaRawEndpoint(SkyfishHttpClient client) {
            Client = client;
        }

        #endregion

        #region Member methods

        /// <summary>
        /// Returns a list of tags (Exif data) of the media with the specified <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The ID of the media.</param>
        /// <returns>An instance of <see cref="IHttpResponse"/> representing the response.</returns>
        public IHttpResponse GetTags(int id) {
            return Client.GetResponse(new SkyfishGetMediaTagsOptions(id));
        }

        /// <summary>
        /// Returns a list of tags (Exif data) of the media identified by the specified <paramref name="options"/>.
        /// </summary>
        /// <param name="options">The options for the request to the API.</param>
        /// <returns>An instance of <see cref="IHttpResponse"/> representing the response.</returns>
        public IHttpResponse GetTags(SkyfishGetMediaTagsOptions options) {
            if (options is null) throw new ArgumentNullException(nameof(options));
            return Client.GetResponse(options);
        }

        #endregion

    }

}