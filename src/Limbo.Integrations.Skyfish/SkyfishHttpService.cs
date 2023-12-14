using System;
using Limbo.Integrations.Skyfish.Endpoints;
using Limbo.Integrations.Skyfish.Http;

namespace Limbo.Integrations.Skyfish;

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
    /// Gets a reference to the <strong>Folders</strong> endpoint.
    /// </summary>
    public SkyfishFoldersEndpoint Folders { get; }

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
        Folders = new SkyfishFoldersEndpoint(this);
        Media = new SkyfishMediaEndpoint(this);
        Search = new SkyfishSearchEndpoint(this);
    }

    #endregion

    #region Static methods

    /// <summary>
    /// Creates and returns a new instance based on the specified <paramref name="token"/>.
    /// </summary>
    /// <param name="token">The token for accessing the API.</param>
    /// <returns>An instance of <see cref="SkyfishHttpService"/>.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="token"/> is null, empty or white space.</exception>
    public static SkyfishHttpService CreateFromKeys(string token) {

        // Input validation
        if (string.IsNullOrWhiteSpace(token)) throw new ArgumentNullException(nameof(token));

        // Initialize a new HTTP client
        SkyfishHttpClient client = new(token);

        // Return a new HTTP service wrapping the client
        return new SkyfishHttpService(client);

    }

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

        // Input validation
        if (string.IsNullOrWhiteSpace(publicKey)) throw new ArgumentNullException(nameof(publicKey));
        if (string.IsNullOrWhiteSpace(secretKey)) throw new ArgumentNullException(nameof(secretKey));
        if (string.IsNullOrWhiteSpace(username)) throw new ArgumentNullException(nameof(username));
        if (string.IsNullOrWhiteSpace(password)) throw new ArgumentNullException(nameof(password));

        // Initialize a new HTTP client
        SkyfishHttpClient client = new(publicKey, secretKey, username, password);

        // Get a new token from the specified parameters
        client.Token = client.GetToken().Body.Token;

        // Return a new HTTP service wrapping the client
        return new SkyfishHttpService(client);

    }

    /// <summary>
    /// Creates and returns a new instance based on the specified <paramref name="client"/>.
    /// </summary>
    /// <param name="client">The HTTP client to be wrapped.</param>
    /// <returns>An instance of <see cref="SkyfishHttpService"/>.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="client"/> is null.</exception>
    public static SkyfishHttpService CreateFromClient(SkyfishHttpClient client) {

        // Input validation
        if (client is null) throw new ArgumentNullException(nameof(client));

        // Return a new HTTP service wrapping the client
        return new SkyfishHttpService(client);

    }

    #endregion

}