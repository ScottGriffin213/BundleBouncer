using Newtonsoft.Json;
namespace BundleBouncer.Model
{

    public class Photon255
    {
        [JsonProperty("249")]
        public Parameter249 _249 { get; set; }

        [JsonProperty("252")]
        public long[] _252 { get; set; }

        [JsonProperty("254")]
        public long _254 { get; set; }
    }

    public class Parameter249
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
