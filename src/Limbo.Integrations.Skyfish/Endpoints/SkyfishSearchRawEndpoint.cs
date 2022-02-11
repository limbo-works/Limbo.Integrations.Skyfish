using System;
using Limbo.Integrations.Skyfish.Http;
using Limbo.Integrations.Skyfish.Options.Videos;
using Skybrud.Essentials.Http;

namespace Limbo.Integrations.Skyfish.Endpoints {
    
    public class SkyfishSearchRawEndpoint {

        public SkyfishHttpClient Client { get; }

        public SkyfishSearchRawEndpoint(SkyfishHttpClient client) {
            Client = client;
        }

        #region Member methods

        public IHttpResponse Search(SkyfishSearchOptions options) {
            if (options == null) throw new ArgumentNullException(nameof(options));
            return Client.GetResponse(options);
        }

        #endregion

    }

}