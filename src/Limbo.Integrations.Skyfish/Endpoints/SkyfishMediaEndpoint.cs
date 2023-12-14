using System.Net;
using Limbo.Integrations.Skyfish.Options.Media;
using Limbo.Integrations.Skyfish.Responses;
using Limbo.Integrations.Skyfish.Responses.Media;
using Skybrud.Essentials.Http;

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

        /// <summary>
        /// Returns the stream URL of the media with the specified <paramref name="uniqueMediaId"/>.
        /// </summary>
        /// <param name="uniqueMediaId">The unique ID of the media.</param>
        /// <returns>An instance of <see cref="IHttpResponse"/> representing the raw response.</returns>
        /// <remarks>
        /// If a stream URL does not yet exist for the media, the API will return a <c>404 Not Found</c> response.
        ///
        /// If a stream is still in the process of being created, the API will return a <see cref="HttpStatusCode.OK"/>
        /// response, but with an empty stream URL.
        /// </remarks>
        public SkyfishMediaStreamUrlResponse GetStreamUrl(int uniqueMediaId) {
            return new SkyfishMediaStreamUrlResponse(Raw.GetStreamUrl(uniqueMediaId));
        }

        /// <summary>
        /// Creates a new stream for the media with the specified <paramref name="uniqueMediaId"/>
        /// </summary>
        /// <param name="uniqueMediaId">The unique ID of the media.</param>
        /// <returns>An instance of <see cref="IHttpResponse"/> representing the raw response.</returns>
        /// <remarks>
        /// Calling this method will only start the creating of the stream. If this step is successful, the API
        /// responds with a <see cref="HttpStatusCode.Created"/> response. After this, the <see cref="GetStreamUrl"/>
        /// can be used to check whether a stream URL is available - eg. by checking each second until available.
        ///
        /// If this method is called, but a stream already exist, the API will return a
        /// <see cref="HttpStatusCode.Conflict"/> response.
        /// </remarks>
        public SkyfishResponse CreateStream(int uniqueMediaId) {
            return new SkyfishResponse(Raw.CreateStream(uniqueMediaId));
        }

        #endregion

    }

}