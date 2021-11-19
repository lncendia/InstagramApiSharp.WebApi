using InstagramApiSharp.Classes.ResponseWrappers;
using InstagramApiSharp.WebApi.Classes.Likes;
using Newtonsoft.Json;

namespace InstagramApiSharp.WebApi.Classes.Comments
{
    public class Comment
    {
        [JsonProperty("pk")] public long Pk { get; set; }
        [JsonProperty("text")] public string Text { get; set; }
        [JsonProperty("created_at")] public long CreatedAt { get; set; }
        [JsonProperty("did_report_as_spam")] public bool ReportedAsSpam { get; set; }
        [JsonProperty("owner")] public InstaUserShortResponse Commenter { get; set; }
        [JsonProperty("viewer_has_liked")] public bool HasLiked { get; set; }

        [JsonProperty("is_restricted_pending")]
        public bool RestrictedPending { get; set; }

        [JsonProperty("edge_liked_by")] public InstaMediaLikesList Likes { get; set; }

        [JsonProperty("edge_threaded_comments")]
        public InstaMediaCommentsList ChildComments { get; set; }
    }
}