using Newtonsoft.Json;
using System.Collections.Generic;
namespace BundleBouncer.Model
{
    public class AvatarDictUnityPackage
    {
        [JsonProperty("assetUrl")]
        public string AssetUrl { get; set; }

        [JsonProperty("assetUrlObject")]
        public Dictionary<string, object> AssetUrlObject { get; set; }

        [JsonProperty("assetVersion")]
        public double AssetVersion { get; set; }

        [JsonProperty("created_at", NullValueHandling = NullValueHandling.Ignore)]
        public string CreatedAt { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("platform")]
        public string Platform { get; set; }

        [JsonProperty("unitySortNumber")]
        public double UnitySortNumber { get; set; }

        [JsonProperty("unityVersion")]
        public string UnityVersion { get; set; }
    }
}
