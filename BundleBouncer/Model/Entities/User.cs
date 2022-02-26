using Newtonsoft.Json;
namespace BundleBouncer.Model
{
    public class User
    {
        [JsonProperty("allowAvatarCopying")]
        public bool AllowAvatarCopying { get; set; }

        [JsonProperty("bio")]
        public string Bio { get; set; }

        [JsonProperty("currentAvatarImageUrl")]
        public string CurrentAvatarImageUrl { get; set; }

        [JsonProperty("currentAvatarThumbnailImageUrl")]
        public string CurrentAvatarThumbnailImageUrl { get; set; }

        [JsonProperty("developerType")]
        public string DeveloperType { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("last_platform")]
        public string LastPlatform { get; set; }

        [JsonProperty("profilePicOverride")]
        public string ProfilePicOverride { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("statusDescription")]
        public string StatusDescription { get; set; }

        [JsonProperty("tags")]
        public string[] Tags { get; set; }

        [JsonProperty("userIcon")]
        public string UserIcon { get; set; }
    }
}
