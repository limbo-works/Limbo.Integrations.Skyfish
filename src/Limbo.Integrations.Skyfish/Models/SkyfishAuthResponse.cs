using Newtonsoft.Json.Linq;
using Skybrud.Essentials.Json.Extensions;

namespace Limbo.Integrations.Skyfish.Models {
    public class SkyfishAuthResponse {
        public long ValidUntilUnix { get; set; }
        public string Token { get; set; }

        public SkyfishAuthResponse(JObject obj) {
            ValidUntilUnix = long.Parse(obj.GetString("validUntil"));
            Token = obj.GetString("token");
        }
    }
}
