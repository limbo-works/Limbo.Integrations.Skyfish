using Limbo.Integrations.Skyfish.Models.Media;
using Skybrud.Essentials.Http;

namespace Limbo.Integrations.Skyfish.Responses.Media {

    public class SkyfishMediaTagsResponse : SkyfishResponse<SkyfishMediaTags> {

        public SkyfishMediaTagsResponse(IHttpResponse response) : base(response) {
            Body = ParseJsonObject(response.Body, SkyfishMediaTags.Parse)!;
        }

    }

}