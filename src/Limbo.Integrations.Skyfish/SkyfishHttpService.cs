using System;
using Limbo.Integrations.Skyfish.Endpoints;
using Limbo.Integrations.Skyfish.Http;

namespace Limbo.Integrations.Skyfish {

    /// <summary>
    /// Class working as an entry point to making requests to the various endpoints of the Skyfish API.
    /// </summary>
    public class SkyfishHttpService {

        #region Properties

        /// <summary>
        /// Gets a reference to the underlying client used for the raw communication.
        /// </summary>
        public SkyfishHttpClient Client { get; }

        /// <summary>
        /// Gets a reference to the <strong>Media</strong> endpoint.
        /// </summary>
        public SkyfishMediaEndpoint Media { get; }

        /// <summary>
        /// Gets a reference to the <strong>Search</strong> endpoint.
        /// </summary>
        public SkyfishSearchEndpoint Search { get; }

        #endregion

        #region Constructors

        private SkyfishHttpService(SkyfishHttpClient client) {
            Client = client;
            Media = new SkyfishMediaEndpoint(this);
            Search = new SkyfishSearchEndpoint(this);
        }

        #endregion

        #region Member methods

        /// <summary>
        /// Returns the embed URL of the media with the specified <paramref name="uniqueMediaId"/>, or <see langword="null"/> if not successful.
        /// </summary>
        /// <param name="uniqueMediaId">The unique ID of the media.</param>
        /// <returns>The embed URL.</returns>
        public string? GetEmbedUrl(int uniqueMediaId) {
            return Client.GetEmbedUrl(uniqueMediaId);
        }

        #endregion

        #region Static methods

        /// <summary>
        /// Creates and returns a new instance based on the specified <paramref name="publicKey"/>, <paramref name="secretKey"/>, <paramref name="username"/> and <paramref name="password"/>.
        /// </summary>
        /// <param name="publicKey">The public key.</param>
        /// <param name="secretKey">The secret key.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>An instance of <see cref="SkyfishHttpService"/>.</returns>
        /// <exception cref="ArgumentNullException">If any parameter is null, empty or white space.</exception>
        public static SkyfishHttpService CreateFromKeys(string publicKey, string secretKey, string username, string password) {
            if (string.IsNullOrWhiteSpace(publicKey)) throw new ArgumentNullException(nameof(publicKey));
            if (string.IsNullOrWhiteSpace(secretKey)) throw new ArgumentNullException(nameof(secretKey));
            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentNullException(nameof(username));
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentNullException(nameof(password));
            return new SkyfishHttpService(new SkyfishHttpClient(publicKey, secretKey, username, password));
        }

        #endregion

    }

}