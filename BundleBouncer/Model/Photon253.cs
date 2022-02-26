using Newtonsoft.Json;
namespace BundleBouncer.Model
{

    public class Photon253
    {
        [JsonProperty("251")]
        public Parameter251 _251 { get; set; }

        [JsonProperty("253")]
        public long _253 { get; set; }

        [JsonProperty("254")]
        public long _254 { get; set; }
    }

    public class Parameter251
    {
        [JsonProperty("avatarDict")]
        public Avatar AvatarDict { get; set; }

        [JsonProperty("avatarEyeHeight")]
        public long AvatarEyeHeight { get; set; }

        [JsonProperty("favatarDict")]
        public Avatar FavatarDict { get; set; }

        [JsonProperty("inVRMode")]
        public bool InVrMode { get; set; }

        [JsonProperty("showSocialRank")]
        public bool ShowSocialRank { get; set; }

        [JsonProperty("user")]
        public User User { get; set; }
    }
}
