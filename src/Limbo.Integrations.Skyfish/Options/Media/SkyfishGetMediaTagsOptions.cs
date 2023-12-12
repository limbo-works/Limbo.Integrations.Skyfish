using Skybrud.Essentials.Common;
using Skybrud.Essentials.Http;
using Skybrud.Essentials.Http.Options;

namespace Limbo.Integrations.Skyfish.Options.Media {

    /// <summary>
    /// Class with options for listing the tags (Efix data) of a media.
    /// </summary>
    public class SkyfishGetMediaTagsOptions : IHttpRequestOptions {

        #region Properties

        /// <summary>
        /// Gets or sets the ID of the media.
        /// </summary>
        public int Id { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance with default options.
        /// </summary>
        public SkyfishGetMediaTagsOptions() { }

        /// <summary>
        /// Initializes a new instance based on the specified <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The ID of the media.</param>
        public SkyfishGetMediaTagsOptions(int id) {
            Id = id;
        }

        #endregion

        #region Member methods

        /// <inheritdoc />
        public IHttpRequest GetRequest() {
            if (Id == 0) throw new PropertyNotSetException(nameof(Id));
            return HttpRequest.Get($"/media/{Id}/tags");
        }

        #endregion

    }

}