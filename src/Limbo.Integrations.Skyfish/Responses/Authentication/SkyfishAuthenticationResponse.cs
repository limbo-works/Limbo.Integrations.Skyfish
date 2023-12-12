using Limbo.Integrations.Skyfish.Models.Authentication;
using Skybrud.Essentials.Http;

namespace Limbo.Integrations.Skyfish.Responses.Authentication {

    /// <summary>
    /// Class representing the response of an authentication request.
    /// </summary>
    public class SkyfishAuthenticationResponse : SkyfishResponse<SkyfishAuthenticationResult> {

        /// <summary>
        /// Initializes a new instance based on the specified <paramref name="response"/>.
        /// </summary>
        /// <param name="response">An instance of <see cref="IHttpResponse"/> representing the raw response.</param>
        public SkyfishAuthenticationResponse(IHttpResponse response) : base(response) {
            Body = ParseJsonObject(response.Body, SkyfishAuthenticationResult.Parse)!;
        }

    }

}