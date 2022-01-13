using Limbo.Integrations.Skyfish.Http;
using Limbo.Integrations.Skyfish.Models;
using System;

namespace Limbo.Integrations.Skyfish
{
    public class SkyfishHttpService
    {
        public SkyfishHttpClient Client { get; set; }
        
        private SkyfishHttpService(SkyfishHttpClient client)
        {
            Client = client;
        }

        public SkyfishVideo GetVideo(int id)
        {
            return Client.GetVideo(id);
        }

        public static SkyfishHttpService CreateFromKeys(string publicKey, string secretKey, string username, string password)
        {
            if (string.IsNullOrWhiteSpace(publicKey)) throw new ArgumentNullException(nameof(publicKey));
            if (string.IsNullOrWhiteSpace(secretKey)) throw new ArgumentNullException(nameof(secretKey));
            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentNullException(nameof(username));
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentNullException(nameof(password));
            return new SkyfishHttpService(new SkyfishHttpClient(publicKey, secretKey, username, password));
        }
    }
}
