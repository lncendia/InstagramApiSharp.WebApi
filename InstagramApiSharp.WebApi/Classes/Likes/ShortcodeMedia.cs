using Newtonsoft.Json;

namespace InstagramApiSharp.GetMediaLikers.Classes.Likes
{
    public class ShortcodeMedia
    {
        [JsonProperty("shortcode_media")] public MediaInfo ShortcodeMediaInfo { get; set; }
    }
}