using System;
using Limbo.Integrations.Skyfish.Http;
using Limbo.Integrations.Skyfish.Options.Folders;
using Skybrud.Essentials.Http;

namespace Limbo.Integrations.Skyfish.Endpoints;

/// <summary>
/// Class representing the implementation of the raw <strong>Folders</strong> endpoint.
/// </summary>
public class SkyfishFoldersRawEndpoint {

    #region Properties

    /// <summary>
    /// Gets a reference to the underlying HTTP client.
    /// </summary>
    public SkyfishHttpClient Client { get; }

    #endregion

    #region Constructors

    internal SkyfishFoldersRawEndpoint(SkyfishHttpClient client) {
        Client = client;
    }

    #endregion

    #region Member methods

    /// <summary>
    /// Returns a list of folders matching the specified <paramref name="options"/>.
    /// </summary>
    /// <param name="options">The options for the request to the API.</param>
    /// <returns>An instance of <see cref="IHttpResponse"/> representing the raw response.</returns>
    public IHttpResponse Search(SkyfishSearchFoldersOptions options) {
        if (options == null) throw new ArgumentNullException(nameof(options));
        return Client.GetResponse(options);
    }

    /// <summary>
    /// Returns information about the folder with the specified <paramref name="folderId"/>.
    /// </summary>
    /// <param name="folderId">The ID of the folder.</param>
    /// <returns>An instance of <see cref="IHttpResponse"/> representing the raw response.</returns>
    public IHttpResponse GetFolder(int folderId) {
        return Client.Get($"/folder/{folderId}");
    }

    #endregion

}