using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json.Linq;
using Skybrud.Essentials.Json.Extensions;
using Skybrud.Essentials.Time;

namespace Limbo.Integrations.Skyfish.Models.Authentication {

    /// <summary>
    /// Class representing the result of an authentication request.
    /// </summary>
    public class SkyfishAuthenticationResult {

        #region Properties

        /// <summary>
        /// Gets the token.
        /// </summary>
        public string Token { get; }

        /// <summary>
        /// Gets an instance of <see cref="EssentialsTime"/> representing the date and time when the token epxires.
        /// </summary>
        public EssentialsTime ValidUntil { get; }

        private SkyfishAuthenticationResult(JObject json) {
            ValidUntil = json.GetInt64("validUntil", EssentialsTime.FromUnixTimeSeconds)!;
            Token = json.GetString("token")!;
        }

        #endregion

        #region Member methods

        /// <summary>
        /// Initializes a new instance of <see cref="SkyfishAuthenticationResult"/> based on the specified <paramref name="json"/> object.
        /// </summary>
        /// <param name="json">The JSON object representing the result.</param>
        /// <returns>An instance of <see cref="SkyfishAuthenticationResult"/>.</returns>
        [return: NotNullIfNotNull("json")]
        public static SkyfishAuthenticationResult? Parse(JObject? json) {
            return json == null ? null : new SkyfishAuthenticationResult(json);
        }

        #endregion

    }

}