using Newtonsoft.Json;
namespace BundleBouncer.Model
{
    public class Avatar
    {
        [JsonProperty("assetUrl")]
        public string AssetUrl { get; set; }

        [JsonProperty("authorId")]
        public string AuthorId { get; set; }

        [JsonProperty("authorName")]
        public string AuthorName { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("featured")]
        public bool Featured { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("releaseStatus")]
        public string ReleaseStatus { get; set; }

        [JsonProperty("tags")]
        public string[] Tags { get; set; }

        [JsonProperty("thumbnailImageUrl")]
        public string ThumbnailImageUrl { get; set; }

        [JsonProperty("unityPackages")]
        public AvatarDictUnityPackage[] UnityPackages { get; set; }

        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }

        [JsonProperty("version")]
        public long Version { get; set; }
    }
}
