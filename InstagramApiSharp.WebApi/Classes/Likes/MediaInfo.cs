using Newtonsoft.Json;

namespace InstagramApiSharp.WebApi.Classes.Likes
{
    public class MediaInfo
    {
        [JsonProperty("id")] public string Id { get; set; }
        [JsonProperty("shortcode")] public string ShortCode { get; set; }
        [JsonProperty("edge_liked_by")] public InstaMediaLikesList Likes { get; set; }
    }
}