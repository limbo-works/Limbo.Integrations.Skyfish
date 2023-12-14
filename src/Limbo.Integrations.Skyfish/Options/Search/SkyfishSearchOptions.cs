using System.Collections.Generic;
using System.Linq;
using Limbo.Integrations.Skyfish.Models.Media;
using Limbo.Integrations.Skyfish.Options.Media;
using Skybrud.Essentials.Http;
using Skybrud.Essentials.Http.Collections;
using Skybrud.Essentials.Http.Options;
using Skybrud.Essentials.Strings.Extensions;

namespace Limbo.Integrations.Skyfish.Options.Search {

    /// <summary>
    /// Class with options for getting a list of videos.
    /// </summary>
    public class SkyfishSearchOptions : IHttpRequestOptions {

        #region Properties

        /// <summary>
        /// Gets or sets the query string that you want to search for, e.g. <c>cat</c>.
        /// </summary>
        public string? Query { get; set; }

        /// <summary>
        /// Gets or sets a list of values (field) to be returned for each media.
        /// </summary>
        public List<string> ReturnValues { get; set; }

        /// <summary>
        /// Gets or sets the number of results. Default is <c>500</c>.
        /// </summary>
        public int? MediaCount { get; set; }

        /// <summary>
        /// How many rows should be skipped from the start of the result. Mainly used for pagination.
        /// </summary>
        public int? MediaOffset { get; set; }

        /// <summary>
        /// Gets or sets the media types to be returned.
        /// </summary>
        public List<SkyfishMediaType> MediaTypes { get; set; } = new();

        /// <summary>
        /// Gets or sets a list of IDs for the folder to search in.
        /// </summary>
        public List<int> FolderIds { get; set; } = new();

        /// <summary>
        /// Gets or sets whether the search for <see cref="FolderIds"/> should be recursive.
        /// </summary>
        public bool? IsRecursive { get; set; }

        /// <summary>
        /// Gets or sets the ID of a specific media to be returned.
        /// </summary>
        public int MediaId { get; set; }

        /// <summary>
        /// Gets or sets the unique ID of a specific media to be returned.
        /// </summary>
        public int UniqueMediaId { get; set; }

        /// <summary>
        /// Order the search result according to either <see cref="SkyfishMediaSortField.Relevance"/>,
        /// <see cref="SkyfishMediaSortField.Created"/> or <see cref="SkyfishMediaSortField.Distance"/>. Created is the time
        /// the media was imported into the Colourbox system.
        ///
        /// If <see cref="SkyfishMediaSortField.Distance"/> is specified, the latitude and longitude must be provided as
        /// parameters as well.
        /// </summary>
        public SkyfishMediaSortField? Order { get; set; }

        /// <summary>
        /// Specify the direction of the ordering. Permissible values are <see cref="SkyfishSortOrder.Ascending"/> or
        /// <see cref="SkyfishSortOrder.Descending"/>. Note that this option can only be given if <see cref="Order"/>
        /// is given as well.
        /// </summary>
        public SkyfishSortOrder? Direction { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance with default options.
        /// </summary>
        public SkyfishSearchOptions() {

            ReturnValues = new List<string> {
                "media_id",
                "unique_media_id",
                "media_type",
                "height",
                "width",
                "title",
                "description",
                "folder_id",
                "thumbnail_url",
                "thumbnail_url_ssl",
                "filename",
                "file_disksize",
                "file_mimetype"
            };

        }

        #endregion

        #region Member methods

        /// <inheritdoc />
        public IHttpRequest GetRequest() {

            // Initialize the query string
            IHttpQueryString query = new HttpQueryString();

            // Append optional parameters if specified
            if (!string.IsNullOrWhiteSpace(Query)) query.Add("q", Query!);
            if (MediaId > 0) query.Add("media_id", MediaId);
            if (UniqueMediaId > 0) query.Add("unique_media_id", UniqueMediaId);
            if (MediaCount is not null) query.Add("media_count", MediaCount);
            if (MediaOffset is not null) query.Add("media_offset", MediaOffset);

            if (FolderIds is { Count: > 0 }) query.Add("folder_ids", string.Join(" ", FolderIds));
            if (IsRecursive is not null) query.Add("recursive", IsRecursive.Value ? "true" : "false");

            if (Order is not null) query.Add("order", Order.Value.ToLower());
            if (Direction is not null) query.Add("direction", Direction.Value == SkyfishSortOrder.Ascending ? "asc" : "desc");

            // The documentation says to split the values with +, but the API doesn't support URL encoded + chars
            // Instead we can split by space and it turns into ASCII + chars ¯\(º_o)/¯
            if (ReturnValues is { Count: > 0 }) query.Add("return_values", string.Join(" ", ReturnValues));
            if (MediaTypes is { Count: > 0 }) query.Add("media_type", string.Join(" ", from type in MediaTypes select type.ToUnderscore()));

            // Initialize a new GET request
            return HttpRequest.Get("/search", query);

        }

        #endregion

    }

}