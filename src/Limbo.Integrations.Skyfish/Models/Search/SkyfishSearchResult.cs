using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Limbo.Integrations.Skyfish.Models.Media;
using Newtonsoft.Json.Linq;
using Skybrud.Essentials.Json.Newtonsoft;
using Skybrud.Essentials.Json.Newtonsoft.Extensions;

namespace Limbo.Integrations.Skyfish.Models.Search {

    public class SkyfishSearchResult : JsonObjectBase {

        public int Hits { get; }

        public IReadOnlyList<SkyfishMediaItem> Media { get; }

        private SkyfishSearchResult(JObject json) : base(json) {
            Hits = json.GetInt32ByPath("response.hits");
            Media = json.GetArrayItemsByPath("response.media", SkyfishMediaItem.Parse);
        }

        [return: NotNullIfNotNull("json")]
        public static SkyfishSearchResult? Parse(JObject? json) {
            return json == null ? null : new SkyfishSearchResult(json);
        }

    }

}