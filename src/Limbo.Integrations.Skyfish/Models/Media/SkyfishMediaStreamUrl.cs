using Newtonsoft.Json.Linq;
using System.Diagnostics.CodeAnalysis;
using Skybrud.Essentials.Json.Newtonsoft.Extensions;

namespace Limbo.Integrations.Skyfish.Models.Media;

/// <summary>
/// Class with information about the stream URL of a Skyfish media.
/// </summary>
public class SkyfishMediaStreamUrl : SkyfishObject {

    #region Properties

    /// <summary>
    /// Gets the stream URL.
    /// </summary>
    public string? Stream { get; }

    #endregion

    /// <summary>
    /// Initializes a new instance based on the specified <paramref name="json"/> object.
    /// </summary>
    /// <param name="json">The JSON object representing the media item.</param>
    protected SkyfishMediaStreamUrl(JObject json) : base(json) {
        Stream = json.GetString("Stream");
    }

    /// <summary>
    /// Initializes a new instance of <see cref="SkyfishMediaStreamUrl"/> based on the specified <paramref name="json"/> object.
    /// </summary>
    /// <param name="json">The JSON object representing the stream URL.</param>
    /// <returns>An instance of <see cref="SkyfishMediaStreamUrl"/>.</returns>
    [return: NotNullIfNotNull("json")]
    public static SkyfishMediaStreamUrl? Parse(JObject? json) {
        return json == null ? null : new SkyfishMediaStreamUrl(json);
    }

}