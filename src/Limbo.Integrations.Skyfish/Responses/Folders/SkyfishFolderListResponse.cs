using System.Collections.Generic;
using Limbo.Integrations.Skyfish.Models.Folders;
using Skybrud.Essentials.Http;

namespace Limbo.Integrations.Skyfish.Responses.Folders;

/// <summary>
/// Class representing the response with a list of Skyfish folder.
/// </summary>
public class SkyfishFolderListResponse : SkyfishResponse<IReadOnlyList<SkyfishFolderItem>> {

    /// <summary>
    /// Initializes a new instance based on the specified <paramref name="response"/>.
    /// </summary>
    /// <param name="response">An instance of <see cref="IHttpResponse"/> representing the raw response.</param>
    public SkyfishFolderListResponse(IHttpResponse response) : base(response) {
        Body = ParseJsonArray(response.Body, SkyfishFolderItem.Parse)!;
    }

}