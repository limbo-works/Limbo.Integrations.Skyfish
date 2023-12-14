using Limbo.Integrations.Skyfish.Options.Folders;
using Limbo.Integrations.Skyfish.Responses.Folders;
using Skybrud.Essentials.Http;

namespace Limbo.Integrations.Skyfish.Endpoints {

    /// <summary>
    /// Class representing the implementation of the <strong>Folders</strong> endpoint.
    /// </summary>
    public class SkyfishFoldersEndpoint {

        #region Properties

        /// <summary>
        /// Gets a reference to the Skyfish service.
        /// </summary>
        public SkyfishHttpService Service { get; }

        /// <summary>
        /// Gets a reference to the raw endpoint.
        /// </summary>
        public SkyfishFoldersRawEndpoint Raw => Service.Client.Folders;

        #endregion

        #region Constructors

        internal SkyfishFoldersEndpoint(SkyfishHttpService service) {
            Service = service;
        }

        #endregion

        #region Member methods

        /// <summary>
        /// Returns a list of folders matching the specified <paramref name="options"/>.
        /// </summary>
        /// <param name="options">The options for the request to the API.</param>
        /// <returns>An instance of <see cref="IHttpResponse"/> representing the response.</returns>
        public SkyfishFolderListResponse Search(SkyfishSearchFoldersOptions options) {
            return new SkyfishFolderListResponse(Raw.Search(options));
        }

        /// <summary>
        /// Returns information about the folder with the specified <paramref name="folderId"/>.
        /// </summary>
        /// <param name="folderId">The ID of the folder.</param>
        /// <returns>An instance of <see cref="SkyfishFolderResponse"/> representing the response.</returns>
        public SkyfishFolderResponse GetFolder(int folderId) {
            return new SkyfishFolderResponse(Raw.GetFolder(folderId));
        }

        #endregion

    }

}