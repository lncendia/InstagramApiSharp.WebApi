using Newtonsoft.Json;

namespace InstagramApiSharp.WebApi.Classes.Comments
{
    public class ShortcodeMedia
    {
        [JsonProperty("shortcode_media")] public MediaInfo ShortcodeMediaInfo { get; set; }
    }
}