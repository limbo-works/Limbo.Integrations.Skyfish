using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json.Linq;
using Skybrud.Essentials.Json.Newtonsoft.Extensions;
using Skybrud.Essentials.Time;

namespace Limbo.Integrations.Skyfish.Models.Authentication;

/// <summary>
/// Class representing the result of a token authentication request.
/// </summary>
public class SkyfishTokenResult : SkyfishObject {

    #region Properties

    /// <summary>
    /// Gets the token.
    /// </summary>
    public string Token { get; }

    /// <summary>
    /// Gets an instance of <see cref="EssentialsTime"/> representing the date and time when the token expires.
    /// </summary>
    public EssentialsTime ValidUntil { get; }

    private SkyfishTokenResult(JObject json) : base(json) {
        ValidUntil = json.GetRequiredInt64("validUntil", EssentialsTime.FromUnixTimeSeconds);
        Token = json.GetRequiredString("token");
    }

    #endregion

    #region Member methods

    /// <summary>
    /// Initializes a new instance of <see cref="SkyfishTokenResult"/> based on the specified <paramref name="json"/> object.
    /// </summary>
    /// <param name="json">The JSON object representing the result.</param>
    /// <returns>An instance of <see cref="SkyfishTokenResult"/>.</returns>
    [return: NotNullIfNotNull(nameof(json))]
    public static SkyfishTokenResult? Parse(JObject? json) {
        return json == null ? null : new SkyfishTokenResult(json);
    }

    #endregion

}