using System;
using Limbo.Integrations.Skyfish.Endpoints;
using Limbo.Integrations.Skyfish.Http;

namespace Limbo.Integrations.Skyfish {
    public class SkyfishHttpService {
        public SkyfishHttpClient Client { get; }

        public SkyfishSearchEndpoint Search { get; }

        private SkyfishHttpService(SkyfishHttpClient client) {
            Client = client;
            Search = new SkyfishSearchEndpoint(this);
        }

        public string GetEmbedUrl(int uniqueMediaId) {
            return Client.GetEmbedUrl(uniqueMediaId);
        }

        public static SkyfishHttpService CreateFromKeys(string publicKey, string secretKey, string username, string password) {
            if (string.IsNullOrWhiteSpace(publicKey)) throw new ArgumentNullException(nameof(publicKey));
            if (string.IsNullOrWhiteSpace(secretKey)) throw new ArgumentNullException(nameof(secretKey));
            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentNullException(nameof(username));
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentNullException(nameof(password));
            return new SkyfishHttpService(new SkyfishHttpClient(publicKey, secretKey, username, password));
        }
    }
}