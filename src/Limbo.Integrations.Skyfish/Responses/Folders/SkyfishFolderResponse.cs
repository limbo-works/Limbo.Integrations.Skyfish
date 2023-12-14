using Limbo.Integrations.Skyfish.Models.Folders;
using Skybrud.Essentials.Http;

namespace Limbo.Integrations.Skyfish.Responses.Folders;

/// <summary>
/// Class representing the response with information about a Skyfish folder.
/// </summary>
public class SkyfishFolderResponse : SkyfishResponse<SkyfishFolder> {

    /// <summary>
    /// Initializes a new instance based on the specified <paramref name="response"/>.
    /// </summary>
    /// <param name="response">An instance of <see cref="IHttpResponse"/> representing the raw response.</param>
    public SkyfishFolderResponse(IHttpResponse response) : base(response) {
        Body = ParseJsonObject(response.Body, SkyfishFolder.Parse)!;
    }

}