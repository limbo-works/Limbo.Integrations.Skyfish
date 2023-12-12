using System.Collections.Generic;
using Limbo.Integrations.Skyfish.Models.Media;
using Newtonsoft.Json.Linq;
using Skybrud.Essentials.Json;
using Skybrud.Essentials.Json.Extensions;

namespace Limbo.Integrations.Skyfish.Models.Search {

    public class SkyfishSearchResult : JsonObjectBase {

        public int Hits { get; }

        public IReadOnlyList<SkyfishMediaItem> Media { get; }

        private SkyfishSearchResult(JObject json) : base(json) {
            Hits = json.GetInt32("response.hits");
            Media = json.GetArrayItems("response.media", SkyfishMediaItem.Parse);
        }

        public static SkyfishSearchResult Parse(JObject json) {
            return json == null ? null : new SkyfishSearchResult(json);
        }

    }

}