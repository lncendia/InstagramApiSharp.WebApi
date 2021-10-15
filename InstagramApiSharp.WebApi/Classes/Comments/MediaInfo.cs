using Newtonsoft.Json;

namespace InstagramApiSharp.GetMediaLikers.Classes.Comments
{
    public class MediaInfo
    {
        [JsonProperty("edge_media_to_parent_comment")] public InstaMediaCommentsList Comments { get; set; }
    }
}