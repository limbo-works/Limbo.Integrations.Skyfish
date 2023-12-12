using System;
using Limbo.Integrations.Skyfish.Http;
using Limbo.Integrations.Skyfish.Options.Media;
using Skybrud.Essentials.Http;

namespace Limbo.Integrations.Skyfish.Endpoints {

    public class SkyfishMediaRawEndpoint {

        public SkyfishHttpClient Client { get; }

        public SkyfishMediaRawEndpoint(SkyfishHttpClient client) {
            Client = client;
        }

        #region Member methods

        public IHttpResponse GetTags(int id) {
            return Client.GetResponse(new SkyfishGetMediaTagsOptions(id));
        }

        public IHttpResponse GetTags(SkyfishGetMediaTagsOptions options) {
            if (options is null) throw new ArgumentNullException(nameof(options));
            return Client.GetResponse(options);
        }

        #endregion

    }

}