using Skybrud.Essentials.Http;
using Limbo.Integrations.Skyfish.Models.Search;

namespace Limbo.Integrations.Skyfish.Responses.Search {

    public class SkyfishSearchResponse : SkyfishResponse<SkyfishSearchResult> {

        public SkyfishSearchResponse(IHttpResponse response) : base(response) {
            Body = ParseJsonObject(response.Body, SkyfishSearchResult.Parse)!;
        }

    }

}