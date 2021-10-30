using Newtonsoft.Json;

namespace InstagramApiSharp.GetMediaLikers.Classes.Likes
{
    public class Edge
    {
        [JsonProperty("node")] public User Like { get; set; }
    }
}