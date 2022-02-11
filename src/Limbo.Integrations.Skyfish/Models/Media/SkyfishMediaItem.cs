using Newtonsoft.Json.Linq;
using Skybrud.Essentials.Json;
using Skybrud.Essentials.Json.Extensions;

namespace Limbo.Integrations.Skyfish.Models.Media {
    
    public class SkyfishMediaItem : JsonObjectBase {

        #region Properties

        public string[] Keywords { get; }

        public int Height { get; }

        public int Width { get; }

        public int UniqueMediaId { get; }

        public string Title { get; }

        public string Description { get; }

        public string ThumbnailUrl { get; }

        public string ThumbnailUrlSsl { get; }

        public string FileName { get; }

        public string FileMimeType { get; }

        public long FileDiskSize { get; }

        #endregion
        
        protected SkyfishMediaItem(JObject json) : base(json) {
            Keywords = json.GetStringArray("keywords");
            Height = json.GetInt32("height");
            Width = json.GetInt32("width");
            UniqueMediaId = json.GetInt32("unique_media_id");
            Title = json.GetString("title");
            Description = json.GetString("description");
            ThumbnailUrl = json.GetString("thumbnail_url");
            ThumbnailUrlSsl = json.GetString("thumbnail_url_ssl");
            FileName = json.GetString("filename");
            FileDiskSize = json.GetInt32("file_disksize");
            FileMimeType = json.GetString("file_mimetype");
        }

        public static SkyfishMediaItem Parse(JObject json) {
            return json == null ? null : new SkyfishMediaItem(json);

        }

    }

}