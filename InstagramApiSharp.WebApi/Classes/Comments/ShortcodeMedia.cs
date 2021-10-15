using Newtonsoft.Json;

namespace InstagramApiSharp.GetMediaLikers.Classes.Comments
{
    public class ShortcodeMedia
    {
        [JsonProperty("shortcode_media")] public MediaInfo ShortcodeMediaInfo { get; set; }
    }
}