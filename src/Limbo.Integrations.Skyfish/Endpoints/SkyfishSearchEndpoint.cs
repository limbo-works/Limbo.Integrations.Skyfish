using Limbo.Integrations.Skyfish.Options.Videos;
using Limbo.Integrations.Skyfish.Responses.Search;

namespace Limbo.Integrations.Skyfish.Endpoints {

    public class SkyfishSearchEndpoint {

        public SkyfishHttpService Service { get; }

        public SkyfishSearchRawEndpoint Raw => Service.Client.Search;

        public SkyfishSearchEndpoint(SkyfishHttpService service) {
            Service = service;
        }

        public SkyfishSearchResponse Search(SkyfishSearchOptions options) {
            return new SkyfishSearchResponse(Raw.Search(options));
        }

    }

}