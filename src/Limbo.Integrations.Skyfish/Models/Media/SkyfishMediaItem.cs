using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json.Linq;
using Skybrud.Essentials.Json.Newtonsoft;
using Skybrud.Essentials.Json.Newtonsoft.Extensions;

namespace Limbo.Integrations.Skyfish.Models.Media {

    /// <summary>
    /// Class representing a Skyfish media item.
    /// </summary>
    public class SkyfishMediaItem : JsonObjectBase {

        #region Properties

        /// <summary>
        /// Gets the ID of the media.
        /// </summary>
        public int MediaId { get; }

        /// <summary>
        /// Gets the unique ID of the media.
        /// </summary>
        public int UniqueMediaId { get; }

        /// <summary>
        /// Gets the media type of the media - eg. <see cref="SkyfishMediaType.Image"/> or <see cref="SkyfishMediaType.Video"/>.
        /// </summary>
        public SkyfishMediaType? MediaType { get; }

        /// <summary>
        /// Gets a list of the keywords of the media.
        /// </summary>
        public IReadOnlyList<string> Keywords { get; }

        /// <summary>
        /// Gets the height of the media.
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// Gets the width of the media.
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// Gets the title of the media.
        /// </summary>
        public string? Title { get; }

        /// <summary>
        /// Gets the description of the media.
        /// </summary>
        public string? Description { get; }

        /// <summary>
        /// Gets the ID of the parent folder.
        /// </summary>
        public int FolderId { get; }

        /// <summary>
        /// Gets the thumbnail URL of the media.
        /// </summary>
        public string? ThumbnailUrl { get; }

        /// <summary>
        /// Gets the secure thumbnail URL of the media.
        /// </summary>
        public string? ThumbnailUrlSsl { get; }

        /// <summary>
        /// Gets the file name of the media.
        /// </summary>
        public string? FileName { get; }

        /// <summary>
        /// Gets the mime type of the media file.
        /// </summary>
        public string? FileMimeType { get; }

        /// <summary>
        /// Gets the size of the media file.
        /// </summary>
        public long FileDiskSize { get; }

        #endregion

        /// <summary>
        /// Initializes a new instance based on the specified <paramref name="json"/> object.
        /// </summary>
        /// <param name="json">The JSON object representing the media item.</param>
        protected SkyfishMediaItem(JObject json) : base(json) {
            MediaId = json.GetInt32("media_id");
            UniqueMediaId = json.GetInt32("unique_media_id");
            MediaType = json.GetEnumOrNull<SkyfishMediaType>("media_type");
            Keywords = json.GetStringArray("keywords");
            Height = json.GetInt32("height");
            Width = json.GetInt32("width");
            Title = json.GetString("title");
            Description = json.GetString("description");
            FolderId = json.GetInt32("folder_id");
            ThumbnailUrl = json.GetString("thumbnail_url");
            ThumbnailUrlSsl = json.GetString("thumbnail_url_ssl");
            FileName = json.GetString("filename");
            FileDiskSize = json.GetInt32("file_disksize");
            FileMimeType = json.GetString("file_mimetype");
        }

        /// <summary>
        /// Initializes a new instance of <see cref="SkyfishMediaItem"/> based on the specified <paramref name="json"/> object.
        /// </summary>
        /// <param name="json">The JSON object representing the media item.</param>
        /// <returns>An instance of <see cref="SkyfishMediaItem"/>.</returns>
        [return: NotNullIfNotNull("json")]
        public static SkyfishMediaItem? Parse(JObject? json) {
            return json == null ? null : new SkyfishMediaItem(json);
        }

    }

}