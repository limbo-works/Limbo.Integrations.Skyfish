using Limbo.Integrations.Skyfish.Endpoints;
using Limbo.Integrations.Skyfish.Responses.Authentication;
using Newtonsoft.Json.Linq;
using Skybrud.Essentials.Common;
using Skybrud.Essentials.Http;
using Skybrud.Essentials.Http.Client;
using Skybrud.Essentials.Security;
using Skybrud.Essentials.Time.UnixTime;

namespace Limbo.Integrations.Skyfish.Http {

    /// <summary>
    /// HTTP client for making HTTP based requests to the Skyfish API.
    /// </summary>
    public class SkyfishHttpClient : HttpClient {

        #region Properties

        /// <summary>
        /// Gets or sets the public key.
        /// </summary>
        public string? PublicKey { get; set; }

        /// <summary>
        /// Gets or sets the secret key.
        /// </summary>
        public string? SecretKey { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string? Username { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string? Password { get; set; }

        /// <summary>
        /// Gets or sets the token used for accessing the SKyfish API.
        /// </summary>
        public string? Token { get; set; }

        /// <summary>
        /// Gets a reference to the raw <strong>Folders</strong> endpoint.
        /// </summary>
        public SkyfishFoldersRawEndpoint Folders { get; }

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
            Folders = new SkyfishFoldersRawEndpoint(this);
            Media = new SkyfishMediaRawEndpoint(this);
            Search = new SkyfishSearchRawEndpoint(this);
        }

        /// <summary>
        /// Initializes a new instance based on the specified <paramref name="token"/>.
        /// </summary>
        /// <param name="token">The token for accessing the API.</param>
        public SkyfishHttpClient(string token) : this() {
            Token = token;
        }

        /// <summary>
        /// Initializes a new instance based  on the specified <paramref name="publicKey"/>, <paramref name="secretKey"/>, <paramref name="username"/> and <paramref name="password"/>.
        /// </summary>
        /// <param name="publicKey">The public key.</param>
        /// <param name="secretKey">The secret key.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        public SkyfishHttpClient(string publicKey, string secretKey, string username, string password) : this() {
            PublicKey = publicKey;
            SecretKey = secretKey;
            Username = username;
            Password = password;
        }

        #endregion

        #region Member methods

        /// <summary>
        /// Returns a new token based on the <see cref="PublicKey"/>, <see cref="SecretKey"/>, <see cref="Username"/>
        /// and <see cref="Password"/> properties.
        /// </summary>
        /// <returns>An instance of <see cref="SkyfishTokenResponse"/> with information about the token.</returns>
        public SkyfishTokenResponse GetToken() {

            if (string.IsNullOrWhiteSpace(PublicKey)) throw new PropertyNotSetException(nameof(PublicKey));
            if (string.IsNullOrWhiteSpace(SecretKey)) throw new PropertyNotSetException(nameof(SecretKey));
            if (string.IsNullOrWhiteSpace(Username)) throw new PropertyNotSetException(nameof(Username));
            if (string.IsNullOrWhiteSpace(Password)) throw new PropertyNotSetException(nameof(Password));

            // HMAC hash needed for authing with Skyfish - https://api.skyfish.com/#sectionHead-21
            int unixTimestamp = (int) UnixTimeUtils.CurrentSeconds;
            string hmac = SecurityUtils.GetHmacSha1Hash(SecretKey!, $"{PublicKey}:{unixTimestamp}");

            // Initialize the request body
            JObject body = new() {
                ["username"] = Username,
                ["password"] = Password,
                ["key"] = PublicKey,
                ["ts"] = unixTimestamp,
                ["hmac"] = hmac.ToLower()
            };

            // Authenticate with Skyfish
            IHttpResponse response = HttpUtils.Requests.Post("https://api.colourbox.com/authenticate/userpasshmac", body);

            // Return the strongly typed response
            return new SkyfishTokenResponse(response);

        }

        /// <inheritdoc />
        protected override void PrepareHttpRequest(IHttpRequest request) {

            // Append the scheme and domain of not already present
            if (request.Url.StartsWith("/")) request.Url = $"https://api.colourbox.com{request.Url}";

            // Set the "Authorization" header if the token is present
            if (!string.IsNullOrWhiteSpace(Token)) request.Authorization = $"CBX-SIMPLE-TOKEN Token={Token}";

        }

        #endregion

    }

}