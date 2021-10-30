using Newtonsoft.Json;

namespace InstagramApiSharp.GetMediaLikers.Classes
{
    public class User
    {
        [JsonProperty("username")] public string UserName { get; set; }

        [JsonProperty("profile_pic_url")] public string ProfilePicture { get; set; }

        [JsonProperty("full_name")] public string FullName { get; set; }

        [JsonProperty("is_verified")] public bool IsVerified { get; set; }

        [JsonProperty("is_private")] public bool IsPrivate { get; set; }

        [JsonProperty("followed_by_viewer")] public bool IsFollowedByViewer { get; set; }
        [JsonProperty("requested_by_viewer")] public bool IsRequestedByViewer { get; set; }

        [JsonProperty("id")] public long Pk { get; set; }
        [JsonProperty("pk")] private long Pk2
        {
            set => Pk = value;
        }
    }
}