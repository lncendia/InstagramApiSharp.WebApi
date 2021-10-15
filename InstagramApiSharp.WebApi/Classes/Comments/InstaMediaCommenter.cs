using Newtonsoft.Json;

namespace InstagramApiSharp.GetMediaLikers.Classes.Comments
{
    public class InstaMediaCommenter
    {
        [JsonProperty("username")] public string UserName { get; set; }

        [JsonProperty("profile_pic_url")] public string ProfilePicture { get; set; }

        [JsonProperty("is_verified")] public bool IsVerified { get; set; }

        [JsonProperty("id")] public long Pk { get; set; }
    }
}