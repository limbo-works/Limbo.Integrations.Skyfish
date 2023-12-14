using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Limbo.Integrations.Skyfish.Models.Media;
using Newtonsoft.Json.Linq;
using Skybrud.Essentials.Json.Newtonsoft.Extensions;

namespace Limbo.Integrations.Skyfish.Models.Search {

    /// <summary>
    /// Class representing the result of a media search.
    /// </summary>
    public class SkyfishSearchResult : SkyfishObject {

        #region Properties

        /// <summary>
        /// Gets the total amount if hits (media) the search matched.
        /// </summary>
        public int Hits { get; }

        /// <summary>
        /// Gets a list of the media returned on the current page.
        /// </summary>
        public IReadOnlyList<SkyfishMediaItem> Media { get; }

        #endregion

        #region Constructors

        private SkyfishSearchResult(JObject json) : base(json) {
            Hits = json.GetInt32ByPath("response.hits");
            Media = json.GetArrayItemsByPath("response.media", SkyfishMediaItem.Parse);
        }

        #endregion

        #region Member methods

        /// <summary>
        /// Initializes a new instance of <see cref="SkyfishMediaItem"/> based on the specified <paramref name="json"/> object.
        /// </summary>
        /// <param name="json">The JSON object representing the search result.</param>
        /// <returns>An instance of <see cref="SkyfishMediaItem"/>.</returns>
        [return: NotNullIfNotNull("json")]
        public static SkyfishSearchResult? Parse(JObject? json) {
            return json == null ? null : new SkyfishSearchResult(json);
        }

        #endregion

    }

}