using Newtonsoft.Json.Linq;
using Skybrud.Essentials.Json.Newtonsoft;

#pragma warning disable CS1591

namespace Limbo.Integrations.Skyfish.Models;

public class SkyfishObject : JsonObjectBase {

    public new JObject JObject => base.JObject!;

    public SkyfishObject(JObject json) : base(json) { }

}