using System.Collections.Generic;
using System.Linq;
using Limbo.Integrations.Skyfish.Models.Media;
using Skybrud.Essentials.Http;
using Skybrud.Essentials.Http.Collections;
using Skybrud.Essentials.Http.Options;
using Skybrud.Essentials.Strings.Extensions;

namespace Limbo.Integrations.Skyfish.Options.Videos {
    
    /// <summary>
    /// Class with options for getting a list of videos.
    /// </summary>
    public class SkyfishSearchOptions : IHttpRequestOptions {

        #region Properties

        /// <summary>
        /// Gets or sets the ID of a specific media to be returned.
        /// </summary>
        public int MediaId { get; set; }
        
        /// <summary>
        /// Gets or sets the unique ID of a specific media to be returned.
        /// </summary>
        public int UniqueMediaId { get; set; }

        /// <summary>
        /// Gets or sets a list of values (field) to be returned for each media.
        /// </summary>
        public List<string> ReturnValues { get; set; }

        /// <summary>
        /// Gets or sets the media types to be returned.
        /// </summary>
        public List<SkyfishMediaType> MediaTypes { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance with default options.
        /// </summary>
        public SkyfishSearchOptions() {
            
            ReturnValues = new List<string> {
                "unique_media_id",
                "height",
                "width",
                "title",
                "description",
                "thumbnail_url",
                "thumbnail_url_ssl",
                "filename+file_disksize",
                "file_mimetype"
            };

            MediaTypes = new List<SkyfishMediaType>();

        }

        #endregion

        #region Member methods

        /// <inheritdoc />
        public IHttpRequest GetRequest() {
            
            // Initialize the query string
            IHttpQueryString query = new HttpQueryString();

            // Append optional parameters if specified
            if (MediaId > 0) query.Add("media_id", MediaId);
            if (UniqueMediaId > 0) query.Add("unique_media_id", UniqueMediaId);
            if (ReturnValues != null && ReturnValues.Count > 0) query.Add("return_values", string.Join("+", query));
            if (MediaTypes != null && MediaTypes.Count > 0) query.Add("media_type", string.Join("+", from type in MediaTypes select type.ToUnderscore()));

            // Initialize a new GET request
            return HttpRequest.Get("/search", query);

        }

        #endregion

    }

}