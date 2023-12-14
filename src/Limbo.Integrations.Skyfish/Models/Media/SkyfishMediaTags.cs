using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json.Linq;
using Skybrud.Essentials.Json.Newtonsoft.Extensions;

#pragma warning disable CS1591

namespace Limbo.Integrations.Skyfish.Models.Media {

    /// <summary>
    /// Class describing the tags (Exif data) of a Skyfish media.
    /// </summary>
    public class SkyfishMediaTags : SkyfishObject {

        #region Properties

        public Dictionary<string, object>? ExifTool { get; }

        public Dictionary<string, object>? Composite { get; }

        public string SourceFile { get; }

        public Dictionary<string, object>? QuickTime { get; }

        public Dictionary<string, object>? File { get; }

        #endregion

        protected SkyfishMediaTags(JObject json) : base(json) {
            ExifTool = json.GetObject<Dictionary<string, object>>("ExifTool");
            Composite = json.GetObject<Dictionary<string, object>>("Composite");
            SourceFile = json.GetString("SourceFile")!;
            QuickTime = json.GetObject<Dictionary<string, object>>("QuickTime");
            File = json.GetObject<Dictionary<string, object>>("File");
        }

        [return: NotNullIfNotNull("json")]
        public static SkyfishMediaTags? Parse(JObject? json) {
            return json == null ? null : new SkyfishMediaTags(json);
        }

    }

}