using System.Runtime.Caching;
using Limbo.Integrations.Skyfish.Endpoints;
using Limbo.Integrations.Skyfish.Models.Authentication;
using Newtonsoft.Json.Linq;
using Skybrud.Essentials.Common;
using Skybrud.Essentials.Http;
using Skybrud.Essentials.Http.Client;
using Skybrud.Essentials.Json.Newtonsoft;
using Skybrud.Essentials.Security;
using Skybrud.Essentials.Time.UnixTime;

namespace Limbo.Integrations.Skyfish.Http {

    /// <summary>
    /// HTTP client for making HTTP based requests to the Skyfish API.
    /// </summary>
    public class SkyfishHttpClient : HttpClient {

        private readonly string? _token;

        #region Properties

        /// <summary>
        /// Gets the public key.
        /// </summary>
        public string? PublicKey { get; }

        /// <summary>
        /// Gets the secret key.
        /// </summary>
        public string? SecretKey { get; }

        /// <summary>
        /// Gets the username.
        /// </summary>
        public string? Username { get; }

        /// <summary>
        /// Gets the password.
        /// </summary>
        public string? Password { get; }

        /// <summary>
        /// Gets a reference to the raw <strong>Media</strong> endpoint.
        /// </summary>
        public SkyfishMediaRawEndpoint Media { get; }

        /// <summary>
        /// Gets a reference to the raw <strong>Search</strong> endpoint.
        /// </summary>
        public SkyfishSearchRawEndpoint Search { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance with default options.
        /// </summary>
        public SkyfishHttpClient() {
            Search = new SkyfishSearchRawEndpoint(this);
            Media = new SkyfishMediaRawEndpoint(this);
        }

        /// <summary>
        /// Initializes a new instance based  on the specified <paramref name="publicKey"/>, <paramref name="secretKey"/>, <paramref name="username"/> and <paramref name="password"/>.
        /// </summary>
        /// <param name="publicKey">The public key.</param>
        /// <param name="secretKey">The secret key.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        public SkyfishHttpClient(string publicKey, string secretKey, string username, string password) {

            PublicKey = publicKey;
            SecretKey = secretKey;
            Username = username;
            Password = password;

            Search = new SkyfishSearchRawEndpoint(this);
            Media = new SkyfishMediaRawEndpoint(this);

            _token = GetToken();

        }

        #endregion

        #region Member methods

        private string? GetToken() {

            // TODO: Should this be public?

            if (string.IsNullOrWhiteSpace(PublicKey)) throw new PropertyNotSetException(nameof(PublicKey));
            if (string.IsNullOrWhiteSpace(SecretKey)) throw new PropertyNotSetException(nameof(SecretKey));
            if (string.IsNullOrWhiteSpace(Username)) throw new PropertyNotSetException(nameof(Username));
            if (string.IsNullOrWhiteSpace(Password)) throw new PropertyNotSetException(nameof(Password));

            // Default token expiration is 14 days, so instead of requesting each time we cache it and get it from cache
            ObjectCache cache = MemoryCache.Default;
            string? token = cache["skyfishToken"] as string;
            if (!string.IsNullOrWhiteSpace(token)) return token;

            // Hmac hash needed for authing with Skyfish - https://api.skyfish.com/#sectionHead-21
            int unixTimestamp = (int) UnixTimeUtils.CurrentSeconds;
            string hmac = SecurityUtils.GetHmacSha1Hash(SecretKey!, $"{PublicKey}:{unixTimestamp}");

            JObject body = new() {
                ["username"] = Username,
                ["password"] = Password,
                ["key"] = PublicKey,
                ["ts"] = unixTimestamp,
                ["hmac"] = hmac.ToLower()
            };

            // Authenticate with Skyfish
            IHttpResponse result = HttpRequest.Post("https://api.colourbox.com/authenticate/userpasshmac", body).GetResponse();

            // Responds with unix expiration timestamp we can use to set the token expiration in cache - and the token itself
            SkyfishAuthenticationResult response = JsonUtils.ParseJsonObject(result.Body, SkyfishAuthenticationResult.Parse)!;

            cache.Set("skyfishToken", response.Token, response.ValidUntil.DateTimeOffset);

            return response.Token;

        }

        /// <inheritdoc />
        protected override void PrepareHttpRequest(IHttpRequest request) {

            // Append the scheme and domain of not already present
            if (request.Url.StartsWith("/")) request.Url = $"https://api.colourbox.com{request.Url}";

            // Set the "Authorization" header if the token is present
            if (!string.IsNullOrWhiteSpace(_token)) request.Authorization = $"CBX-SIMPLE-TOKEN Token={_token}";

        }

        #endregion

    }

}