using Newtonsoft.Json.Linq;
using Skybrud.Essentials.Json;
using Skybrud.Essentials.Json.Extensions;

namespace Limbo.Integrations.Skyfish.Models {
    public class SkyfishVideo : JsonObjectBase {
        /// <summary>
        /// Gets the numeric ID of the video.
        /// </summary>
        public int VideoId { get; }

        /// <summary>
        /// Gets the height of the video
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// Gets the width of the video
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// Gets the title of the video.
        /// </summary>
        public string VideoTitle { get; }

        /// <summary>
        /// Gets the description of the video.
        /// </summary>
        public string VideoDescription { get; }

        /// <summary>
        /// Gets the video's thumbnail URL.
        /// </summary>
        public string ThumbnailUrl { get; }

        /// <summary>
        /// Gets the video's SSL thumbnail URL.
        /// </summary>
        public string ThumbnailUrlSsl { get; }

        /// <summary>
        /// Gets the original file name of the video file.
        /// </summary>
        public string FileName { get; }

        /// <summary>
        /// Gets the video filesize in bytes.
        /// This is the upload size, may differ from download size as they do some compression.
        /// </summary>
        public int FileSize { get; }

        /// <summary>
        /// Get file mime type.
        /// </summary>
        public string FileMimeType { get; }

        /// <summary>
        /// Gets the iframe src url.
        /// </summary>
        public string EmbedUrl { get; set; }

        protected SkyfishVideo(JObject obj) : base(obj) {
            VideoId = obj.GetInt32("response.media[0].unique_media_id");
            Height = obj.GetInt32("response.media[0].height");
            Width = obj.GetInt32("response.media[0].width");
            VideoTitle = obj.GetString("response.media[0].title");
            VideoDescription = obj.GetString("response.media[0].description");
            ThumbnailUrl = obj.GetString("response.media[0].thumbnail_url");
            ThumbnailUrlSsl = obj.GetString("response.media[0].thumbnail_url_ssl");
            FileName = obj.GetString("response.media[0].filename");
            FileSize = obj.GetInt32("response.media[0].file_disksize");
            FileMimeType = obj.GetString("response.media[0].file_mimetype");
        }

        public static SkyfishVideo Parse(JObject obj) {
            return obj == null ? null : new SkyfishVideo(obj);
        }
    }
}
