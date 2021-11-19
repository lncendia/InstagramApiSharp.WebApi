using Newtonsoft.Json;

namespace InstagramApiSharp.WebApi.Classes.Likes
{
    public class ShortcodeMedia
    {
        [JsonProperty("shortcode_media")] public MediaInfo ShortcodeMediaInfo { get; set; }
    }
}