﻿using System.Collections.Generic;
using System.Linq;
using Limbo.Integrations.Skyfish.Models.Media;
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
        /// Gets or sets the ID of a specific media to be returned.
        /// </summary>
        public int MediaId { get; set; }

        /// <summary>
        /// Gets or sets the unique ID of a specific media to be returned.
        /// </summary>
        public int UniqueMediaId { get; set; }

        /// <summary>
        /// Gets or sets a list of IDs for the folder to search in.
        /// </summary>
        public List<int> FolderIds { get; set; } = new();

        /// <summary>
        /// Gets or sets whether the search for <see cref="FolderIds"/> should be recursive.
        /// </summary>
        public bool? IsRecursive { get; set; }

        /// <summary>
        /// Gets or sets a list of values (field) to be returned for each media.
        /// </summary>
        public List<string> ReturnValues { get; set; }

        /// <summary>
        /// Gets or sets the media types to be returned.
        /// </summary>
        public List<SkyfishMediaType> MediaTypes { get; set; } = new();

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
            if (MediaId > 0) query.Add("media_id", MediaId);
            if (UniqueMediaId > 0) query.Add("unique_media_id", UniqueMediaId);

            if (FolderIds is { Count: > 0 }) query.Add("folder_ids", string.Join(" ", FolderIds));
            if (IsRecursive is not null) query.Add("recursive", IsRecursive.Value ? "true" : "false");

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